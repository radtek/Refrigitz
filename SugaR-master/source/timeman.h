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

#ifndef TIMEMAN_H_INCLUDED
#define TIMEMAN_H_INCLUDED

#include "m==c.h"
#include "search.h"
#include "thread.h"

/// The TimeManagement class computes the optimal time to think depending on
/// the maximum available time, the game move number and other parameters.

class TimeManagement {
public:
  void init(Search::LimitsType& limits, Color us, int ply);
  TimePoint optimum() const { return optimumTime; }
  TimePoint maximum() const { return maximumTime; }
  TimePoint elapsed() const { return Search::Limits.npmsec ?
                                     TimePoint(Threads.nodes_searched()) : now() - startTime; }

  int64_t availableNodes; // When in 'nodes as time' mode

private:
  TimePoint startTime;
  TimePoint optimumTime;
  TimePoint maximumTime;
};

extern TimeManagement Time;

#endif // #ifndef TIMEMAN_H_INCLUDED
