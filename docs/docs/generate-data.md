---
layout: default
title: Generating the training data
parent: Documentation
nav_order: 1
has_children: false
---

# Generating the training data

Neural networks require data to be trained on. Chesster utilizes two neural networks, one for piece detection and one for board orientation detection. In order for these networks to properly function, the library provides methods to generate training data.

## Piece training data

The training data for the pieces consists of images of chess pieces placed on various backgrounds. Multiple spritesheets and background variants are used to ensure that the network learns the shape of the piece, and the difference between piece and background.

![Piece Training Data Example]({{site.baseurl}}/assets/images/piece-examples.png)

Piece training data is essentially a permutation of spritesheet tiles and backgrounds. A spritesheet contains 13 pieces (6 black, 6 white, 1 empty), which means that _n_ spritesheets and _m_ backgrounds result in _13 * n * m_ training data files.

### Generating the data

The `PieceImageGenerator` provides the required generation functionality. It requires the target tile size (in pixels), an array of spritesheets, an array of backgrounds and a path to output the images to.

```cs
new PieceImageGenerator(64, 
    IO.GetFiles(IO.SpritesheetsPath, "*.png|*.jpg"), 
    IO.GetFiles(IO.BackgroundsPath, "*.png|*.jpg"))
    .Generate(IO.PieceTrainingDataPath);
```

The `IO.GetFiles(...)` method yields all the files in the specified directory that match with any of the input filters. In general, the `IO` class provides static access to default paths and directory management.

The resulting files are placed in per-piece subdirectories in the specified output path.

### Custom data

This system can easily be extended with custom spritesheets and backgrounds.

Spritesheets are required to contain 13 square tiles in a horizontal line. The total size of the spritesheet is irrelevant, only the aspect ratio of 13:1 is mandatory and will throw an exception if violated. The pieces themselves need to be layed out as follows: _none, whiteknight, whitebishop, whiterook, whitequeen, whiteking, blackknight, blackbishop, blackrook, blackqueen, blackking, blackqueen_. Note the preceding empty tile.

![Spritesheet Example]({{site.baseurl}}/assets/images/spritesheet.png)

Backgrounds are required to have a 1:1 aspect ratio, with an irrelevant total size. Backgrounds should be passed as single images, see the example below.

![Background Example]({{site.baseurl}}/assets/images/background.png)

## Orientation training data

Orientation training data, as opposed to piece training data, contains a degree of randomness. The input of the generator requires a list of [FEN positions](https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation), which are then inverted at random and projected into a CSV file. This is done to teach the network whether the board is viewed from white's or black's perspective, based on the perceived position.

```
r1bqk2r/2ppbppp/p1n2n2/1p2p3/4P3/1B3N2/PPPP1PPP/RNBQR1K1 b - - 0 13
rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b - - 0 14
r4k1r/q2n1p2/1p3Bp1/3p4/5P1P/1P1B4/P1Q5/1K1R3R b - - 0 29
rq2k2r/pp1nbpp1/2p1pnp1/2Pp4/3P3P/2N1PP2/PPQB2P1/1K1R1B1R b kq - 0 14
```

Note that the FEN notation only requires the pieces and no further information on the active color, castling or halfmoves. Including these will simply lead to them being ignored.

### Generating the data

This time the `BoardOrientationGenerator` class does all the magic. It requires an input file of FEN positions and the path to a CSV output file.

```cs
new BoardOrientationGenerator(IO.Combine(IO.AssetRoot, "fens.txt"))
    .Generate(IO.BoardOrientationTrainingDataPath)
```

The resulting CSV file will include the new position, together with a boolean value, stating if the board is inverted or not. Note that the position is no longer stored in a FEN format, but rather as an array of floating point numbers, with each number representing a piece. Negative numbers are black, positive numbers are white, with the pieces becoming more "important" the further they are away from 0, e.g. the King has the value 1 or -1.

```
True,-0.5,0,0,-1,-0.8,-0.35,0,-0.5,-0.1,-0.1,-0.1,-0.35,-0.1,-0.1,0,0,0,0,-0.3,0,0,-0.3,0,-0.1,0,0,0,-0.1,0,0,-0.1,0,0,0,0,0.1,0,0,0,0,0,0,0.3,0,0,0,0.35,0,0.1,0.1,0.1,0,0.1,0.1,0.1,0.1,0,1,0,0.5,0.8,0.35,0.3,0.5
True,-0.5,-0.3,-0.35,-1,-0.8,-0.35,-0.3,-0.5,-0.1,-0.1,-0.1,-0.1,-0.1,-0.1,-0.1,-0.1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.1,0.1,0.1,0.1,0.1,0.1,0.1,0.1,0.5,0.3,0.35,1,0.8,0.35,0.3,0.5
False,0,1,0,0.5,0,0,0,0.5,0.1,0,0.8,0,0,0,0,0,0,0.1,0,0.35,0,0,0,0,0,0,0,0,0,0.1,0,0.1,0,0,0,-0.1,0,0,0,0,0,-0.1,0,0,0,0.35,-0.1,0,-0.8,0,0,-0.3,0,-0.1,0,0,-0.5,0,0,0,0,-1,0,-0.5
False,0,1,0,0.5,0,0.35,0,0.5,0.1,0.1,0.8,0.35,0,0,0.1,0,0,0,0.3,0,0.1,0.1,0,0,0,0,0,0.1,0,0,0,0.1,0,0,0.1,-0.1,0,0,0,0,0,0,-0.1,0,-0.1,-0.3,-0.1,0,-0.1,-0.1,0,-0.3,-0.35,-0.1,-0.1,0,-0.5,-0.8,0,0,-1,0,0,-0.5
```

### Custom data

Sending custom data to the generator is as simple as compiling a list of FEN positions yourself and providing the path of the file to the generator. Automatically generating FEN positions is planned in the future, to allow a wider range of training data.

---

Once the training data is generated, we can continue with [training the models](./training)