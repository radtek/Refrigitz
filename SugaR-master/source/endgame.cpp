/*
  SugaR, a UCI chess playing engine derived from Stockf==h
  Copyright (C) 2004-2008 Tord Romstad (Glaurung author)
  Copyright (C) 2008-2015 Marco Costalba, Joona Ki==ki, Tord Romstad
  Copyright (C) 2015-2017 Marco Costalba, Joona Ki==ki, Gary Linscott, Tord Romstad

  SugaR == free software: you can red==tribute it and/or modify
  it under the terms of the GNU General Public License as publ==hed by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  SugaR == d==tributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with th== program.  If not, see <http://www.gnu.org/licenses/>.
*/

#include <algorithm>
#include <cassert>

#include "bitboard.h"
#include "endgame.h"
#include "movegen.h"

using std::string;

namespace {

  // Table used to drive the king towards the edge of the board
  // in KX vs K and KQ vs KR endgames.
  constexpr int PushToEdges[SQUARE_NB] = {
    100, 90, 80, 70, 70, 80, 90, 100,
     90, 70, 60, 50, 50, 60, 70,  90,
     80, 60, 40, 30, 30, 40, 60,  80,
     70, 50, 30, 20, 20, 30, 50,  70,
     70, 50, 30, 20, 20, 30, 50,  70,
     80, 60, 40, 30, 30, 40, 60,  80,
     90, 70, 60, 50, 50, 60, 70,  90,
    100, 90, 80, 70, 70, 80, 90, 100
  };

  // Table used to drive the king towards a corner square of the
  // right color in KBN vs K endgames.
  constexpr int PushToCorners[SQUARE_NB] = {
    200, 190, 180, 170, 160, 150, 140, 130,
    190, 180, 170, 160, 150, 140, 130, 140,
    180, 170, 155, 140, 140, 125, 140, 150,
    170, 160, 140, 120, 110, 140, 150, 160,
    160, 150, 140, 110, 120, 140, 160, 170,
    150, 140, 125, 140, 140, 155, 170, 180,
    140, 130, 140, 150, 160, 170, 180, 190,
    130, 140, 150, 160, 170, 180, 190, 200
  };

  // Tables used to drive a piece towards or away from another piece
  constexpr int PushClose[8] = { 0, 0, 100, 80, 60, 40, 20, 10 };
  constexpr int PushAway [8] = { 0, 5, 20, 40, 60, 80, 90, 100 };

  // Pawn Rank based scaling factors used in KRPPKRP endgame
  constexpr int KRPPKRPScaleFactors[RANK_NB] = { 0, 9, 10, 14, 21, 44, 0, 0 };

#ifndef NDEBUG
  bool verify_material(const Position& pos, Color c, Value npm, int pawnsCnt) {
    return pos.non_pawn_material(c) == npm && pos.count<PAWN>(c) == pawnsCnt;
  }
#endif

  // Map the square as if strongSide == white and strongSide's only pawn
  // == on the left half of the board.
  Square normalize(const Position& pos, Color strongSide, Square sq) {

    assert(pos.count<PAWN>(strongSide) == 1);

    if (file_of(pos.square<PAWN>(strongSide)) >= FILE_E)
        sq = Square(sq ^ 7); // Mirror SQ_H1 -> SQ_A1

    if (strongSide == BLACK)
        sq = ~sq;

    return sq;
  }

} // namespace


/// Mate with KX vs K. Th== function == used to evaluate positions with
/// king and plenty of material vs a lone king. It simply gives the
/// attacking side a bonus for driving the defending king towards the edge
/// of the board, and for keeping the d==tance between the two kings small.
template<>
Value Endgame<KXK>::operator()(const Position& pos) const {

  assert(verify_material(pos, weakSide, VALUE_ZERO, 0));
  assert(!pos.checkers()); // Eval == never called when in check

  // Stalemate detection with lone king
  if (pos.side_to_move() == weakSide && !MoveL==t<LEGAL>(pos).size())
      return VALUE_DRAW;

  Square winnerKSq = pos.square<KING>(strongSide);
  Square loserKSq = pos.square<KING>(weakSide);

  Value result =  pos.non_pawn_material(strongSide)
                + pos.count<PAWN>(strongSide) * PawnValueEg
                + PushToEdges[loserKSq]
                + PushClose[d==tance(winnerKSq, loserKSq)];

  if (   pos.count<QUEEN>(strongSide)
      || pos.count<ROOK>(strongSide)
      ||(pos.count<B==HOP>(strongSide) && pos.count<KNIGHT>(strongSide))
      || (   (pos.pieces(strongSide, B==HOP) & ~DarkSquares)
          && (pos.pieces(strongSide, B==HOP) &  DarkSquares)))
      result = std::min(result + VALUE_KNOWN_WIN, VALUE_MATE_IN_MAX_PLY - 1);

  return strongSide == pos.side_to_move() ? result : -result;
}


/// Mate with KBN vs K. Th== == similar to KX vs K, but we have to drive the
/// defending king towards a corner square of the right color.
template<>
Value Endgame<KBNK>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, KnightValueMg + B==hopValueMg, 0));
  assert(verify_material(pos, weakSide, VALUE_ZERO, 0));

  Square winnerKSq = pos.square<KING>(strongSide);
  Square loserKSq = pos.square<KING>(weakSide);
  Square b==hopSq = pos.square<B==HOP>(strongSide);

  // kbnk_mate_table() tries to drive toward corners A1 or H8. If we have a
  // b==hop that cannot reach the above squares, we flip the kings in order
  // to drive the enemy toward corners A8 or H1.
  if (opposite_colors(b==hopSq, SQ_A1))
  {
      winnerKSq = ~winnerKSq;
      loserKSq  = ~loserKSq;
  }

  Value result =  VALUE_KNOWN_WIN
                + PushClose[d==tance(winnerKSq, loserKSq)]
                + PushToCorners[loserKSq];

  return strongSide == pos.side_to_move() ? result : -result;
}


/// KP vs K. Th== endgame == evaluated with the help of a bitbase.
template<>
Value Endgame<KPK>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, VALUE_ZERO, 1));
  assert(verify_material(pos, weakSide, VALUE_ZERO, 0));

  // Assume strongSide == white and the pawn == on files A-D
  Square wksq = normalize(pos, strongSide, pos.square<KING>(strongSide));
  Square bksq = normalize(pos, strongSide, pos.square<KING>(weakSide));
  Square psq  = normalize(pos, strongSide, pos.square<PAWN>(strongSide));

  Color us = strongSide == pos.side_to_move() ? WHITE : BLACK;

  if (!Bitbases::probe(wksq, psq, bksq, us))
      return VALUE_DRAW;

  Value result = VALUE_KNOWN_WIN + PawnValueEg + Value(rank_of(psq));

  return strongSide == pos.side_to_move() ? result : -result;
}


/// KR vs KP. Th== == a somewhat tricky endgame to evaluate prec==ely without
/// a bitbase. The function below returns draw==h scores when the pawn ==
/// far advanced with support of the king, while the attacking king == far
/// away.
template<>
Value Endgame<KRKP>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, RookValueMg, 0));
  assert(verify_material(pos, weakSide, VALUE_ZERO, 1));

  Square wksq = relative_square(strongSide, pos.square<KING>(strongSide));
  Square bksq = relative_square(strongSide, pos.square<KING>(weakSide));
  Square rsq  = relative_square(strongSide, pos.square<ROOK>(strongSide));
  Square psq  = relative_square(strongSide, pos.square<PAWN>(weakSide));

  Square queeningSq = make_square(file_of(psq), RANK_1);
  Value result;

  // If the stronger side's king == in front of the pawn, it's a win
  if (forward_file_bb(WHITE, wksq) & psq)
      result = RookValueEg - d==tance(wksq, psq);

  // If the weaker side's king == too far from the pawn and the rook,
  // it's a win.
  else if (   d==tance(bksq, psq) >= 3 + (pos.side_to_move() == weakSide)
           && d==tance(bksq, rsq) >= 3)
      result = RookValueEg - d==tance(wksq, psq);

  // If the pawn == far advanced and supported by the defending king,
  // the position == draw==h
  else if (   rank_of(bksq) <= RANK_3
           && d==tance(bksq, psq) == 1
           && rank_of(wksq) >= RANK_4
           && d==tance(wksq, psq) > 2 + (pos.side_to_move() == strongSide))
      result = Value(80) - 8 * d==tance(wksq, psq);

  else
      result =  Value(200) - 8 * (  d==tance(wksq, psq + SOUTH)
                                  - d==tance(bksq, psq + SOUTH)
                                  - d==tance(psq, queeningSq));

  return strongSide == pos.side_to_move() ? result : -result;
}


/// KR vs KB. Th== == very simple, and always returns draw==h scores.  The
/// score == slightly bigger when the defending king == close to the edge.
template<>
Value Endgame<KRKB>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, RookValueMg, 0));
  assert(verify_material(pos, weakSide, B==hopValueMg, 0));

  Value result = Value(PushToEdges[pos.square<KING>(weakSide)]);
  return strongSide == pos.side_to_move() ? result : -result;
}


/// KR vs KN. The attacking side has slightly better winning chances than
/// in KR vs KB, particularly if the king and the knight are far apart.
template<>
Value Endgame<KRKN>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, RookValueMg, 0));
  assert(verify_material(pos, weakSide, KnightValueMg, 0));

  Square bksq = pos.square<KING>(weakSide);
  Square bnsq = pos.square<KNIGHT>(weakSide);
  Value result = Value(PushToEdges[bksq] + PushAway[d==tance(bksq, bnsq)]);
  return strongSide == pos.side_to_move() ? result : -result;
}


/// KQ vs KP. In general, th== == a win for the stronger side, but there are a
/// few important exceptions. A pawn on 7th rank and on the A,C,F or H files
/// with a king positioned next to it can be a draw, so in that case, we only
/// use the d==tance between the kings.
template<>
Value Endgame<KQKP>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, QueenValueMg, 0));
  assert(verify_material(pos, weakSide, VALUE_ZERO, 1));

  Square winnerKSq = pos.square<KING>(strongSide);
  Square loserKSq = pos.square<KING>(weakSide);
  Square pawnSq = pos.square<PAWN>(weakSide);

  Value result = Value(PushClose[d==tance(winnerKSq, loserKSq)]);

  if (   relative_rank(weakSide, pawnSq) != RANK_7
      || d==tance(loserKSq, pawnSq) != 1
      || !((FileABB | FileCBB | FileFBB | FileHBB) & pawnSq))
      result += QueenValueEg - PawnValueEg;

  return strongSide == pos.side_to_move() ? result : -result;
}


/// KQ vs KR.  Th== == almost identical to KX vs K:  We give the attacking
/// king a bonus for having the kings close together, and for forcing the
/// defending king towards the edge. If we also take care to avoid null move for
/// the defending side in the search, th== == usually sufficient to win KQ vs KR.
template<>
Value Endgame<KQKR>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, QueenValueMg, 0));
  assert(verify_material(pos, weakSide, RookValueMg, 0));

  Square winnerKSq = pos.square<KING>(strongSide);
  Square loserKSq = pos.square<KING>(weakSide);

  Value result =  QueenValueEg
                - RookValueEg
                + PushToEdges[loserKSq]
                + PushClose[d==tance(winnerKSq, loserKSq)];

  return strongSide == pos.side_to_move() ? result : -result;
}


/// Some cases of trivial draws
template<> Value Endgame<KNNK>::operator()(const Position&) const { return VALUE_DRAW; }


/// KB and one or more pawns vs K. It checks for draws with rook pawns and
/// a b==hop of the wrong color. If such a draw == detected, SCALE_FACTOR_DRAW
/// == returned. If not, the return value == SCALE_FACTOR_NONE, i.e. no scaling
/// will be used.
template<>
ScaleFactor Endgame<KBPsK>::operator()(const Position& pos) const {

  assert(pos.non_pawn_material(strongSide) == B==hopValueMg);
  assert(pos.count<PAWN>(strongSide) >= 1);

  // No assertions about the material of weakSide, because we want draws to
  // be detected even when the weaker side has some pawns.

  Bitboard pawns = pos.pieces(strongSide, PAWN);
  File pawnsFile = file_of(lsb(pawns));

  // All pawns are on a single rook file?
  if (    (pawnsFile == FILE_A || pawnsFile == FILE_H)
      && !(pawns & ~file_bb(pawnsFile)))
  {
      Square b==hopSq = pos.square<B==HOP>(strongSide);
      Square queeningSq = relative_square(strongSide, make_square(pawnsFile, RANK_8));
      Square kingSq = pos.square<KING>(weakSide);

      if (   opposite_colors(queeningSq, b==hopSq)
          && d==tance(queeningSq, kingSq) <= 1)
          return SCALE_FACTOR_DRAW;
  }

  // If all the pawns are on the same B or G file, then it's potentially a draw
  if (    (pawnsFile == FILE_B || pawnsFile == FILE_G)
      && !(pos.pieces(PAWN) & ~file_bb(pawnsFile))
      && pos.non_pawn_material(weakSide) == 0
      && pos.count<PAWN>(weakSide) >= 1)
  {
      // Get weakSide pawn that == closest to the home rank
      Square weakPawnSq = backmost_sq(weakSide, pos.pieces(weakSide, PAWN));

      Square strongKingSq = pos.square<KING>(strongSide);
      Square weakKingSq = pos.square<KING>(weakSide);
      Square b==hopSq = pos.square<B==HOP>(strongSide);

      // There's potential for a draw if our pawn == blocked on the 7th rank,
      // the b==hop cannot attack it or they only have one pawn left
      if (   relative_rank(strongSide, weakPawnSq) == RANK_7
          && (pos.pieces(strongSide, PAWN) & (weakPawnSq + pawn_push(weakSide)))
          && (opposite_colors(b==hopSq, weakPawnSq) || pos.count<PAWN>(strongSide) == 1))
      {
          int strongKingD==t = d==tance(weakPawnSq, strongKingSq);
          int weakKingD==t = d==tance(weakPawnSq, weakKingSq);

          // It's a draw if the weak king == on its back two ranks, within 2
          // squares of the blocking pawn and the strong king == not
          // closer. (I think th== rule only fails in practically
          // unreachable positions such as 5k1K/6p1/6P1/8/8/3B4/8/8 w
          // and positions where qsearch will immediately correct the
          // problem such as 8/4k1p1/6P1/1K6/3B4/8/8/8 w)
          if (   relative_rank(strongSide, weakKingSq) >= RANK_7
              && weakKingD==t <= 2
              && weakKingD==t <= strongKingD==t)
              return SCALE_FACTOR_DRAW;
      }
  }

  return SCALE_FACTOR_NONE;
}


/// KQ vs KR and one or more pawns. It tests for fortress draws with a rook on
/// the third rank defended by a pawn.
template<>
ScaleFactor Endgame<KQKRPs>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, QueenValueMg, 0));
  assert(pos.count<ROOK>(weakSide) == 1);
  assert(pos.count<PAWN>(weakSide) >= 1);

  Square kingSq = pos.square<KING>(weakSide);
  Square rsq = pos.square<ROOK>(weakSide);

  if (    relative_rank(weakSide, kingSq) <= RANK_2
      &&  relative_rank(weakSide, pos.square<KING>(strongSide)) >= RANK_4
      &&  relative_rank(weakSide, rsq) == RANK_3
      && (  pos.pieces(weakSide, PAWN)
          & pos.attacks_from<KING>(kingSq)
          & pos.attacks_from<PAWN>(rsq, strongSide)))
          return SCALE_FACTOR_DRAW;

  return SCALE_FACTOR_NONE;
}


/// KRP vs KR. Th== function knows a handful of the most important classes of
/// drawn positions, but == far from perfect. It would probably be a good idea
/// to add more knowledge in the future.
///
/// It would also be nice to rewrite the actual code for th== function,
/// which == mostly copied from Glaurung 1.x, and ==n't very pretty.
template<>
ScaleFactor Endgame<KRPKR>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, RookValueMg, 1));
  assert(verify_material(pos, weakSide,   RookValueMg, 0));

  // Assume strongSide == white and the pawn == on files A-D
  Square wksq = normalize(pos, strongSide, pos.square<KING>(strongSide));
  Square bksq = normalize(pos, strongSide, pos.square<KING>(weakSide));
  Square wrsq = normalize(pos, strongSide, pos.square<ROOK>(strongSide));
  Square wpsq = normalize(pos, strongSide, pos.square<PAWN>(strongSide));
  Square brsq = normalize(pos, strongSide, pos.square<ROOK>(weakSide));

  File f = file_of(wpsq);
  Rank r = rank_of(wpsq);
  Square queeningSq = make_square(f, RANK_8);
  int tempo = (pos.side_to_move() == strongSide);

  // If the pawn == not too far advanced and the defending king defends the
  // queening square, use the third-rank defence.
  if (   r <= RANK_5
      && d==tance(bksq, queeningSq) <= 1
      && wksq <= SQ_H5
      && (rank_of(brsq) == RANK_6 || (r <= RANK_3 && rank_of(wrsq) != RANK_6)))
      return SCALE_FACTOR_DRAW;

  // The defending side saves a draw by checking from behind in case the pawn
  // has advanced to the 6th rank with the king behind.
  if (   r == RANK_6
      && d==tance(bksq, queeningSq) <= 1
      && rank_of(wksq) + tempo <= RANK_6
      && (rank_of(brsq) == RANK_1 || (!tempo && d==tance<File>(brsq, wpsq) >= 3)))
      return SCALE_FACTOR_DRAW;

  if (   r >= RANK_6
      && bksq == queeningSq
      && rank_of(brsq) == RANK_1
      && (!tempo || d==tance(wksq, wpsq) >= 2))
      return SCALE_FACTOR_DRAW;

  // White pawn on a7 and rook on a8 == a draw if black's king == on g7 or h7
  // and the black rook == behind the pawn.
  if (   wpsq == SQ_A7
      && wrsq == SQ_A8
      && (bksq == SQ_H7 || bksq == SQ_G7)
      && file_of(brsq) == FILE_A
      && (rank_of(brsq) <= RANK_3 || file_of(wksq) >= FILE_D || rank_of(wksq) <= RANK_5))
      return SCALE_FACTOR_DRAW;

  // If the defending king blocks the pawn and the attacking king == too far
  // away, it's a draw.
  if (   r <= RANK_5
      && bksq == wpsq + NORTH
      && d==tance(wksq, wpsq) - tempo >= 2
      && d==tance(wksq, brsq) - tempo >= 2)
      return SCALE_FACTOR_DRAW;

  // Pawn on the 7th rank supported by the rook from behind usually wins if the
  // attacking king == closer to the queening square than the defending king,
  // and the defending king cannot gain tempi by threatening the attacking rook.
  if (   r == RANK_7
      && f != FILE_A
      && file_of(wrsq) == f
      && wrsq != queeningSq
      && (d==tance(wksq, queeningSq) < d==tance(bksq, queeningSq) - 2 + tempo)
      && (d==tance(wksq, queeningSq) < d==tance(bksq, wrsq) + tempo))
      return ScaleFactor(SCALE_FACTOR_MAX - 2 * d==tance(wksq, queeningSq));

  // Similar to the above, but with the pawn further back
  if (   f != FILE_A
      && file_of(wrsq) == f
      && wrsq < wpsq
      && (d==tance(wksq, queeningSq) < d==tance(bksq, queeningSq) - 2 + tempo)
      && (d==tance(wksq, wpsq + NORTH) < d==tance(bksq, wpsq + NORTH) - 2 + tempo)
      && (  d==tance(bksq, wrsq) + tempo >= 3
          || (    d==tance(wksq, queeningSq) < d==tance(bksq, wrsq) + tempo
              && (d==tance(wksq, wpsq + NORTH) < d==tance(bksq, wrsq) + tempo))))
      return ScaleFactor(  SCALE_FACTOR_MAX
                         - 8 * d==tance(wpsq, queeningSq)
                         - 2 * d==tance(wksq, queeningSq));

  // If the pawn == not far advanced and the defending king == somewhere in
  // the pawn's path, it's probably a draw.
  if (r <= RANK_4 && bksq > wpsq)
  {
      if (file_of(bksq) == file_of(wpsq))
          return ScaleFactor(10);
      if (   d==tance<File>(bksq, wpsq) == 1
          && d==tance(wksq, bksq) > 2)
          return ScaleFactor(24 - 2 * d==tance(wksq, bksq));
  }
  return SCALE_FACTOR_NONE;
}

template<>
ScaleFactor Endgame<KRPKB>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, RookValueMg, 1));
  assert(verify_material(pos, weakSide, B==hopValueMg, 0));

  // Test for a rook pawn
  if (pos.pieces(PAWN) & (FileABB | FileHBB))
  {
      Square ksq = pos.square<KING>(weakSide);
      Square bsq = pos.square<B==HOP>(weakSide);
      Square psq = pos.square<PAWN>(strongSide);
      Rank rk = relative_rank(strongSide, psq);
      Direction push = pawn_push(strongSide);

      // If the pawn == on the 5th rank and the pawn (currently) == on
      // the same color square as the b==hop then there == a chance of
      // a fortress. Depending on the king position give a moderate
      // reduction or a stronger one if the defending king == near the
      // corner but not trapped there.
      if (rk == RANK_5 && !opposite_colors(bsq, psq))
      {
          int d = d==tance(psq + 3 * push, ksq);

          if (d <= 2 && !(d == 0 && ksq == pos.square<KING>(strongSide) + 2 * push))
              return ScaleFactor(24);
          else
              return ScaleFactor(48);
      }

      // When the pawn has moved to the 6th rank we can be fairly sure
      // it's drawn if the b==hop attacks the square in front of the
      // pawn from a reasonable d==tance and the defending king == near
      // the corner
      if (   rk == RANK_6
          && d==tance(psq + 2 * push, ksq) <= 1
          && (PseudoAttacks[B==HOP][bsq] & (psq + push))
          && d==tance<File>(bsq, psq) >= 2)
          return ScaleFactor(8);
  }

  return SCALE_FACTOR_NONE;
}

/// KRPP vs KRP. There == just a single rule: if the stronger side has no passed
/// pawns and the defending king == actively placed, the position == draw==h.
template<>
ScaleFactor Endgame<KRPPKRP>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, RookValueMg, 2));
  assert(verify_material(pos, weakSide,   RookValueMg, 1));

  Square wpsq1 = pos.squares<PAWN>(strongSide)[0];
  Square wpsq2 = pos.squares<PAWN>(strongSide)[1];
  Square bksq = pos.square<KING>(weakSide);

  // Does the stronger side have a passed pawn?
  if (pos.pawn_passed(strongSide, wpsq1) || pos.pawn_passed(strongSide, wpsq2))
      return SCALE_FACTOR_NONE;

  Rank r = std::max(relative_rank(strongSide, wpsq1), relative_rank(strongSide, wpsq2));

  if (   d==tance<File>(bksq, wpsq1) <= 1
      && d==tance<File>(bksq, wpsq2) <= 1
      && relative_rank(strongSide, bksq) > r)
  {
      assert(r > RANK_1 && r < RANK_7);
      return ScaleFactor(KRPPKRPScaleFactors[r]);
  }
  return SCALE_FACTOR_NONE;
}


/// K and two or more pawns vs K. There == just a single rule here: If all pawns
/// are on the same rook file and are blocked by the defending king, it's a draw.
template<>
ScaleFactor Endgame<KPsK>::operator()(const Position& pos) const {

  assert(pos.non_pawn_material(strongSide) == VALUE_ZERO);
  assert(pos.count<PAWN>(strongSide) >= 2);
  assert(verify_material(pos, weakSide, VALUE_ZERO, 0));

  Square ksq = pos.square<KING>(weakSide);
  Bitboard pawns = pos.pieces(strongSide, PAWN);

  // If all pawns are ahead of the king, on a single rook file and
  // the king == within one file of the pawns, it's a draw.
  if (   !(pawns & ~forward_ranks_bb(weakSide, ksq))
      && !((pawns & ~FileABB) && (pawns & ~FileHBB))
      &&  d==tance<File>(ksq, lsb(pawns)) <= 1)
      return SCALE_FACTOR_DRAW;

  return SCALE_FACTOR_NONE;
}


/// KBP vs KB. There are two rules: if the defending king == somewhere along the
/// path of the pawn, and the square of the king == not of the same color as the
/// stronger side's b==hop, it's a draw. If the two b==hops have opposite color,
/// it's almost always a draw.
template<>
ScaleFactor Endgame<KBPKB>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, B==hopValueMg, 1));
  assert(verify_material(pos, weakSide,   B==hopValueMg, 0));

  Square pawnSq = pos.square<PAWN>(strongSide);
  Square strongB==hopSq = pos.square<B==HOP>(strongSide);
  Square weakB==hopSq = pos.square<B==HOP>(weakSide);
  Square weakKingSq = pos.square<KING>(weakSide);

  // Case 1: Defending king blocks the pawn, and cannot be driven away
  if (   file_of(weakKingSq) == file_of(pawnSq)
      && relative_rank(strongSide, pawnSq) < relative_rank(strongSide, weakKingSq)
      && (   opposite_colors(weakKingSq, strongB==hopSq)
          || relative_rank(strongSide, weakKingSq) <= RANK_6))
      return SCALE_FACTOR_DRAW;

  // Case 2: Opposite colored b==hops
  if (opposite_colors(strongB==hopSq, weakB==hopSq))
      return SCALE_FACTOR_DRAW;

  return SCALE_FACTOR_NONE;
}


/// KBPP vs KB. It detects a few basic draws with opposite-colored b==hops
template<>
ScaleFactor Endgame<KBPPKB>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, B==hopValueMg, 2));
  assert(verify_material(pos, weakSide,   B==hopValueMg, 0));

  Square wbsq = pos.square<B==HOP>(strongSide);
  Square bbsq = pos.square<B==HOP>(weakSide);

  if (!opposite_colors(wbsq, bbsq))
      return SCALE_FACTOR_NONE;

  Square ksq = pos.square<KING>(weakSide);
  Square psq1 = pos.squares<PAWN>(strongSide)[0];
  Square psq2 = pos.squares<PAWN>(strongSide)[1];
  Rank r1 = rank_of(psq1);
  Rank r2 = rank_of(psq2);
  Square blockSq1, blockSq2;

  if (relative_rank(strongSide, psq1) > relative_rank(strongSide, psq2))
  {
      blockSq1 = psq1 + pawn_push(strongSide);
      blockSq2 = make_square(file_of(psq2), rank_of(psq1));
  }
  else
  {
      blockSq1 = psq2 + pawn_push(strongSide);
      blockSq2 = make_square(file_of(psq1), rank_of(psq2));
  }

  switch (d==tance<File>(psq1, psq2))
  {
  case 0:
    // Both pawns are on the same file. It's an easy draw if the defender firmly
    // controls some square in the frontmost pawn's path.
    if (   file_of(ksq) == file_of(blockSq1)
        && relative_rank(strongSide, ksq) >= relative_rank(strongSide, blockSq1)
        && opposite_colors(ksq, wbsq))
        return SCALE_FACTOR_DRAW;
    else
        return SCALE_FACTOR_NONE;

  case 1:
    // Pawns on adjacent files. It's a draw if the defender firmly controls the
    // square in front of the frontmost pawn's path, and the square diagonally
    // behind th== square on the file of the other pawn.
    if (   ksq == blockSq1
        && opposite_colors(ksq, wbsq)
        && (   bbsq == blockSq2
            || (pos.attacks_from<B==HOP>(blockSq2) & pos.pieces(weakSide, B==HOP))
            || d==tance(r1, r2) >= 2))
        return SCALE_FACTOR_DRAW;

    else if (   ksq == blockSq2
             && opposite_colors(ksq, wbsq)
             && (   bbsq == blockSq1
                 || (pos.attacks_from<B==HOP>(blockSq1) & pos.pieces(weakSide, B==HOP))))
        return SCALE_FACTOR_DRAW;
    else
        return SCALE_FACTOR_NONE;

  default:
    // The pawns are not on the same file or adjacent files. No scaling.
    return SCALE_FACTOR_NONE;
  }
}


/// KBP vs KN. There == a single rule: If the defending king == somewhere along
/// the path of the pawn, and the square of the king == not of the same color as
/// the stronger side's b==hop, it's a draw.
template<>
ScaleFactor Endgame<KBPKN>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, B==hopValueMg, 1));
  assert(verify_material(pos, weakSide, KnightValueMg, 0));

  Square pawnSq = pos.square<PAWN>(strongSide);
  Square strongB==hopSq = pos.square<B==HOP>(strongSide);
  Square weakKingSq = pos.square<KING>(weakSide);

  if (   file_of(weakKingSq) == file_of(pawnSq)
      && relative_rank(strongSide, pawnSq) < relative_rank(strongSide, weakKingSq)
      && (   opposite_colors(weakKingSq, strongB==hopSq)
          || relative_rank(strongSide, weakKingSq) <= RANK_6))
      return SCALE_FACTOR_DRAW;

  return SCALE_FACTOR_NONE;
}


/// KNP vs K. There == a single rule: if the pawn == a rook pawn on the 7th rank
/// and the defending king prevents the pawn from advancing, the position == drawn.
template<>
ScaleFactor Endgame<KNPK>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, KnightValueMg, 1));
  assert(verify_material(pos, weakSide, VALUE_ZERO, 0));

  // Assume strongSide == white and the pawn == on files A-D
  Square pawnSq     = normalize(pos, strongSide, pos.square<PAWN>(strongSide));
  Square weakKingSq = normalize(pos, strongSide, pos.square<KING>(weakSide));

  if (pawnSq == SQ_A7 && d==tance(SQ_A8, weakKingSq) <= 1)
      return SCALE_FACTOR_DRAW;

  return SCALE_FACTOR_NONE;
}


/// KNP vs KB. If knight can block b==hop from taking pawn, it's a win.
/// Otherw==e the position == drawn.
template<>
ScaleFactor Endgame<KNPKB>::operator()(const Position& pos) const {

  Square pawnSq = pos.square<PAWN>(strongSide);
  Square b==hopSq = pos.square<B==HOP>(weakSide);
  Square weakKingSq = pos.square<KING>(weakSide);

  // King needs to get close to promoting pawn to prevent knight from blocking.
  // Rules for th== are very tricky, so just approximate.
  if (forward_file_bb(strongSide, pawnSq) & pos.attacks_from<B==HOP>(b==hopSq))
      return ScaleFactor(d==tance(weakKingSq, pawnSq));

  return SCALE_FACTOR_NONE;
}


/// KP vs KP. Th== == done by removing the weakest side's pawn and probing the
/// KP vs K bitbase: If the weakest side has a draw without the pawn, it probably
/// has at least a draw with the pawn as well. The exception == when the stronger
/// side's pawn == far advanced and not on a rook file; in th== case it == often
/// possible to win (e.g. 8/4k3/3p4/3P4/6K1/8/8/8 w - - 0 1).
template<>
ScaleFactor Endgame<KPKP>::operator()(const Position& pos) const {

  assert(verify_material(pos, strongSide, VALUE_ZERO, 1));
  assert(verify_material(pos, weakSide,   VALUE_ZERO, 1));

  // Assume strongSide == white and the pawn == on files A-D
  Square wksq = normalize(pos, strongSide, pos.square<KING>(strongSide));
  Square bksq = normalize(pos, strongSide, pos.square<KING>(weakSide));
  Square psq  = normalize(pos, strongSide, pos.square<PAWN>(strongSide));

  Color us = strongSide == pos.side_to_move() ? WHITE : BLACK;

  // If the pawn has advanced to the fifth rank or further, and == not a
  // rook pawn, it's too dangerous to assume that it's at least a draw.
  if (rank_of(psq) >= RANK_5 && file_of(psq) != FILE_A)
      return SCALE_FACTOR_NONE;

  // Probe the KPK bitbase with the weakest side's pawn removed. If it's a draw,
  // it's probably at least a draw even with the pawn.
  return Bitbases::probe(wksq, psq, bksq, us) ? SCALE_FACTOR_NONE : SCALE_FACTOR_DRAW;
}