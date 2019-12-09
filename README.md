# Game AI Homework 7: Othello

## Team

* Eric Partridge
* Glenn Smith
* Kevin Zhan

## Implementation

Our implementation contains both Negamax and Negamax AB. You can change which AI each player is using with `Player <Player Number> Script Class Name`. Negamax is `NegamaxAI`, and Negamax AB is `NegamaxABAI`. Simply play the game as normal and it should work fine.

## Framework Notes

* The default coupling between BoardScript and AIScript is weird. I suggest changing `GetPointsChangedFromMove` and `GetValidMoves` to static so they are accessible for the AIScript. A way to get turn number would be nice too, or at least a built in helper for detect which player is currently playing.
* The use of Key Value pair makes for messy syntax and general confusion. Mixing up `x` and `y` is easy. I would use Vector2D.
* A way to make a move on an input specified board would be nice, as the AI needs to simulate moves without actually making a move.
* The given framework had issues with detecting when a GameOver occurred, as in it didn't really work at all.
