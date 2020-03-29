---
layout: default
title: Home
nav_order: 1
description: "Just the Docs is a responsive Jekyll theme with built-in search that is easily customizable and hosted on GitHub Pages."
permalink: /
---

# From Screenshot to Chessboard
{: .fs-9 .fw-100 }

Chesster is a .NET Core library with functionality to take a simple screenshot of a chessboard to a fully-evaluated position.
{: .fs-6 .fw-300 }

[//]: # (The desktop client is not available yet.)
[//]: # [Download the desktop client](##download){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 .mr-2 }

[View it on GitHub](https://github.com/yiliansource/chesster){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 }

---

## Introduction

There have been multiple applications for neural networks surrounding the game of chess. [AlphaZero](https://deepmind.com/blog/article/alphazero-shedding-new-light-grand-games-chess-shogi-and-go) revolutionized the way we see modern chess, completely annhilating existing engines. Many papers on the recognition of analog chessboards exist, with fairly high confidence levels.

Chesster is not supposed to be a new step in this direction, but rather a step back to see how far we have come, putting some existing technology and knowledge together. A screenshot of a digital chessboard can be processed to extract the board, predict the pieces and evaluate the best possible move.

The library was written in .NET Core to allow cross-platform usage.

## Installing

For the moment, the library can be cloned via [GitHub](https://github.com/YilianSource/chesster). A NuGet release is planned for the future.

To utilize the neural networks, you first need to train them. You can [refer to the documentation](/documentation/) to find out how to train the models, or you can download the pre-trained models from the [GitHub releases](https://github.com/YilianSource/chesster/releases) (tbd).

## Documentation

### Using the library

1. [Generating the training data](./docs/generate-data)
2. [Training the models](./docs/training)
3. [Evaluating the models](./docs/evaluating)
4. [Predicting the board](./docs/predicting)
   1. [Pieces](./docs/predicting/pieces)
   2. [Orientation](./docs/predicting/orientation)
   3. [Evaluation](./docs/predicting/evaluation)
5. [Combining the steps](./docs/combining)
6. [Logging](./docs/logging)

### Algorithm Breakdown

1. Board Extraction

## Examples

TODO