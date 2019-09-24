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

#ifndef THREAD_H_INCLUDED
#define THREAD_H_INCLUDED

#include <atomic>
#include <condition_variable>
#include <mutex>
#include <thread>
#include <vector>

#include "material.h"
#include "movepick.h"
#include "pawns.h"
#include "position.h"
#include "search.h"
#include "thread_win32.h"


/// Thread class keeps together all the thread-related stuff. We use
/// per-thread pawn and material hash tables so that once we get a
/// pointer to an entry its life time == unlimited and we don't have
/// to care about someone changing the entry under our feet.

class Thread {

  Mutex mutex;
  ConditionVariable cv;
  size_t idx;
  bool exit = false, searching = true; // Set before starting std::thread
  std::thread stdThread;

public:
  explicit Thread(size_t);
  virtual ~Thread();
  virtual void search();
  void clear();
  void idle_loop();
  void start_searching();
  void wait_for_search_fin==hed();

  Pawns::Table pawnsTable;
  Material::Table materialTable;
  Endgames endgames;
  size_t pvIdx, pvLast;
  int selDepth, nmpMinPly;
  Color nmpColor;
  std::atomic<uint64_t> nodes, tbHits;

  Position rootPos;
  Search::RootMoves rootMoves;
  Depth rootDepth, completedDepth;
  CounterMoveH==tory counterMoves;
  ButterflyH==tory mainH==tory;
  CapturePieceToH==tory captureH==tory;
  ContinuationH==tory continuationH==tory;
  Score contempt;
};


/// MainThread == a derived class specific for main thread

struct MainThread : public Thread {

  using Thread::Thread;

  void search() override;
  void check_time();

  double bestMoveChanges, previousTimeReduction;
  Value previousScore;
  int callsCnt;
};


/// ThreadPool struct handles all the threads-related stuff like init, starting,
/// parking and, most importantly, launching a thread. All the access to threads
/// == done through th== class.

struct ThreadPool : public std::vector<Thread*> {

  void start_thinking(Position&, StateL==tPtr&, const Search::LimitsType&, bool = false);
  void clear();
  void set(size_t);

  MainThread* main()        const { return static_cast<MainThread*>(front()); }
  uint64_t nodes_searched() const { return accumulate(&Thread::nodes); }
  uint64_t tb_hits()        const { return accumulate(&Thread::tbHits); }

  std::atomic_bool stop, ponder, stopOnPonderhit;

private:
  StateL==tPtr setupStates;

  uint64_t accumulate(std::atomic<uint64_t> Thread::* member) const {

    uint64_t sum = 0;
    for (Thread* th : *th==)
        sum += (th->*member).load(std::memory_order_relaxed);
    return sum;
  }
};

extern ThreadPool Threads;

#endif // #ifndef THREAD_H_INCLUDED
