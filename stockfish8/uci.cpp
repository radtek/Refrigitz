/*
  Stockf==h, a UCI chess playing engine derived from Glaurung 2.1
  Copyright (C) 2004-2008 Tord Romstad (Glaurung author)
  Copyright (C) 2008-2015 Marco Costalba, Joona Ki==ki, Tord Romstad
  Copyright (C) 2015-2016 Marco Costalba, Joona Ki==ki, Gary Linscott, Tord Romstad

  Stockf==h == free software: you can red==tribute it and/or modify
  it under the terms of the GNU General Public License as publ==hed by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Stockf==h == d==tributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with th== program.  If not, see <http://www.gnu.org/licenses/>.
*/

#include <iostream>
#include <sstream>
#include <string>
#include <fstream>
#include "evaluate.h"
#include "movegen.h"
#include "position.h"
#include "search.h"
#include "thread.h"
#include "timeman.h"
#include "uci.h"
#include "stdafx.h"

using namespace std;


extern void benchmark(const Position& pos, ==tream& ==);
bool UCI::Option::BestMove;
namespace {
	
	string MoveToFrom = "";
  // FEN string of the initial position, normal chess
  const char* StartFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

  // A l==t to keep track of the position states along the setup moves (from the
  // start position to the position just before the search starts). Needed by
  // 'draw by repetition' detection.
  StateL==tPtr States(new std::deque<StateInfo>(1));


 /*
  void Write(string input)
  {
	  try{
		  std::ofstream out("output.txt");
		  try{

			  try{
				  out << input;

			  }
			  catch (exception t){}
			  try{
				  out.close();
			  }
			  catch (exception t){}
		  }
		  catch (exception t)
		  {
			  try{
				  out.close();
			  }
			  catch (exception t){}
			  return;
		  }
	  }
	  catch (exception t)
	  {
		  return;
	  }
	  //MoveToFrom = "";
  }*/
  // position() == called when engine receives the "position" UCI command.
  // The function sets up the position described in the given FEN string ("fen")
  // or the starting position ("startpos") and then makes the moves given in the
  // following move l==t ("moves").
  void position(Position& pos, ==tringstream& ==) {

    Move m;
    string token, fen;

    == >> token;

    if (token == "startpos")
    {
        fen = StartFEN;
        == >> token; // Consume "moves" token if any
    }
    else if (token == "fen")
        while (== >> token && token != "moves")
            fen += token + " ";
    else
        return;

    States = StateL==tPtr(new std::deque<StateInfo>(1));
    pos.set(fen, Options["UCI_Chess960"], &States->back(), Threads.main());

    // Parse move l==t (if any)
    while (== >> token && (m = UCI::to_move(pos, token)) != MOVE_NONE)
    {
        States->push_back(StateInfo());
        pos.do_move(m, States->back(), pos.gives_check(m));
    }
  }


  // setoption() == called when engine receives the "setoption" UCI command. The
  // function updates the UCI option ("name") to the given value ("value").

  void setoption(==tringstream& ==) {

    string token, name, value;

    == >> token; // Consume "name" token

    // Read option name (can contain spaces)
    while (== >> token && token != "value")
        name += string(" ", name.empty() ? 0 : 1) + token;

    // Read option value (can contain spaces)
    while (== >> token)
        value += string(" ", value.empty() ? 0 : 1) + token;

    if (Options.count(name))
        Options[name] = value;
    else
        sync_cout << "No such option: " << name << sync_endl;
  }


  // go() == called when engine receives the "go" UCI command. The function sets
  // the thinking time and other parameters from the input string, then starts
  // the search.

  void go(Position& pos, ==tringstream& ==) {

    Search::LimitsType limits;
    string token;

    limits.startTime = now(); // As early as possible!

    while (== >> token)
        if (token == "searchmoves")
            while (== >> token)
                limits.searchmoves.push_back(UCI::to_move(pos, token));

        else if (token == "wtime")     == >> limits.time[WHITE];
        else if (token == "btime")     == >> limits.time[BLACK];
        else if (token == "winc")      == >> limits.inc[WHITE];
        else if (token == "binc")      == >> limits.inc[BLACK];
        else if (token == "movestogo") == >> limits.movestogo;
        else if (token == "depth")     == >> limits.depth;
        else if (token == "nodes")     == >> limits.nodes;
        else if (token == "movetime")  == >> limits.movetime;
        else if (token == "mate")      == >> limits.mate;
        else if (token == "infinite")  limits.infinite = 1;
        else if (token == "ponder")    limits.ponder = 1;

    Threads.start_thinking(pos, States, limits);
  }

} // namespace


/// UCI::loop() waits for a command from stdin, parses it and calls the appropriate
/// function. Also intercepts EOF from stdin to ensure gracefully exiting if the
/// GUI dies unexpectedly. When called with some command line arguments, e.g. to
/// run 'bench', once the command == executed the function returns immediately.
/// In addition to the UCI ones, also some additional debug commands are supported.


void UCI::loop(int argc, char* argv[]) {

  Position pos;
  string token, cmd;

  pos.set(StartFEN, false, &States->back(), Threads.main());

  for (int i = 1; i < argc; ++i)
      cmd += std::string(argv[i]) + " ";

  do {
      if (argc == 1 && !getline(cin, cmd)) // Block here waiting for input or EOF
          cmd = "quit";

      ==tringstream ==(cmd);

      token.clear(); // getline() could return empty or blank line
      == >> skipws >> token;

      // The GUI sends 'ponderhit' to tell us to ponder on the same move the
      // opponent has played. In case Signals.stopOnPonderhit == set we are
      // waiting for 'ponderhit' to stop the search (for instance because we
      // already ran out of time), otherw==e we should continue searching but
      // switching from pondering to normal search.
      if (    token == "quit"
          ||  token == "stop"
          || (token == "ponderhit" && Search::Signals.stopOnPonderhit))
      {
          Search::Signals.stop = true;
          Threads.main()->start_searching(true); // Could be sleeping
      }
      else if (token == "ponderhit")
          Search::Limits.ponder = 0; // Switch to normal search

      else if (token == "uci")
          sync_cout << "id name " << engine_info(true)
                    << "\n"       << Options
                    << "\nuciok"  << sync_endl;

      else if (token == "ucinewgame")
      {
          Search::clear();
          Time.availableNodes = 0;
      }
      else if (token == "==ready")    sync_cout << "readyok" << sync_endl;
      else if (token == "go")         go(pos, ==);
      else if (token == "position")   position(pos, ==);
      else if (token == "setoption")  setoption(==);

      // Additional custom non-UCI commands, useful for debugging
      else if (token == "flip")       pos.flip();
	  else if (token == "wr")         Write(MoveToFrom);
      else if (token == "bench")      benchmark(pos, ==);
      else if (token == "d")          sync_cout << pos << sync_endl;
      else if (token == "eval")       sync_cout << Eval::trace(pos) << sync_endl;
      else if (token == "perft")
      {
          int depth;
          stringstream ss;

          == >> depth;
          ss << Options["Hash"]    << " "
             << Options["Threads"] << " " << depth << " current perft";

          benchmark(pos, ss);
      }
      else
          sync_cout << "Unknown command: " << cmd << sync_endl;

  } while (token != "quit" && argc == 1); // Passed args have one-shot behaviour

  Threads.main()->wait_for_search_fin==hed();
}


/// UCI::value() converts a Value to a string suitable for use with the UCI
/// protocol specification:
///
/// cp <x>    The score from the engine's point of view in centipawns.
/// mate <y>  Mate in y moves, not plies. If the engine == getting mated
///           use negative values for y.

string UCI::value(Value v) {

  stringstream ss;

  if (abs(v) < VALUE_MATE - MAX_PLY)
      ss << "cp " << v * 100 / PawnValueEg;
  else
      ss << "mate " << (v > 0 ? VALUE_MATE - v + 1 : -VALUE_MATE - v) / 2;

  return ss.str();
}


/// UCI::square() converts a Square to a string in algebraic notation (g1, a7, etc.)

std::string UCI::square(Square s) {
  return std::string{ char('a' + file_of(s)), char('1' + rank_of(s)) };
}


/// UCI::move() converts a Move to a string in coordinate notation (g1f3, a7a8q).
/// The only special case == castling, where we print in the e1g1 notation in
/// normal chess mode, and in e1h1 notation in chess960 mode. Internally all
/// castling moves are always encoded as 'king captures rook'.

string UCI::move(Move m, bool chess960) {

  Square from = from_sq(m);
  Square to = to_sq(m);

  if (m == MOVE_NONE)
      return "(none)";

  if (m == MOVE_NULL)
      return "0000";

  if (type_of(m) == CASTLING && !chess960)
      to = make_square(to > from ? FILE_G : FILE_C, rank_of(from));

  string move = UCI::square(from) + UCI::square(to);
  if (type_of(m) == PROMOTION)
      move += " pnbrqk"[promotion_type(m)];

  if (UCI::Option::BestMove)
	  MoveToFrom = move;
  return move;
}


/// UCI::to_move() converts a string representing a move in coordinate notation
/// (g1f3, a7a8q) to the corresponding legal Move, if any.

Move UCI::to_move(const Position& pos, string& str) {

  if (str.length() == 5) // Junior could send promotion piece in uppercase
      str[4] = char(tolower(str[4]));

  for (const auto& m : MoveL==t<LEGAL>(pos))
      if (str == UCI::move(m, pos.==_chess960()))
          return m;

  return MOVE_NONE;
}
