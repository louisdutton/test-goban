/** The status of a cell on the board/goban. */
const enum Status {
  /** The cell contains a white stone. */
  White = 1,
  /** The cell contains a black stone. */
  Black = 2,
  /** The cell exists on the board but does not contain a stone. */
  Empty = 3,
  /** The cell does not exist within the boundaries of the board. */
  Out = 4,
}

/** A character representation of a {@link Status} (excludes `Status.Out`). */
type StatusRepresentation = "." | "o" | "#";

/** A variable-dimension matrix representation composed of rows of character arrays.  */
type GobanMatrix = string[];

// prettier-ignore
const statusDictionary: Record<StatusRepresentation, Status> = {
  ".": Status.Empty,
  "o": Status.White,
  "#": Status.Black,
};

export default class Goban {
  constructor(private goban: GobanMatrix) {}

  /**
   * Get the status of a given position
   *  @param x - the x coordinate
   *  @param y - the y coordinate
   *  @returns the status of the position
   */
  getStatus(x: number, y: number): Status {
    if (x < 0 || y < 0 || y >= this.goban.length || x >= this.goban[0].length) {
      return Status.Out;
    } else {
      return statusDictionary[this.goban[y][x]];
    }
  }

  /**
   *  Returns true if the stone at the given position is taken.
   *  @param x - the x coordinate of the stone
   *  @param y - the y coordinate of the stone
   *  @returns whether the target stone, or any of its neighbors, has a liberty.
   */
  isTaken(x: number, y: number) {
    const checked = new Set<string>();

    const hasLiberty = (x: number, y: number) => {
      const stone = this.getStatus(x, y);
      const neighbors = [
        [x + 1, y],
        [x - 1, y],
        [x, y + 1],
        [x, y - 1],
      ];

      for (const [nx, ny] of neighbors) {
        if (checked.has(`${nx},${ny}`)) continue;

        const status = this.getStatus(nx, ny);

        if (status === Status.Empty) return true;
        if (status === stone) {
          checked.add(`${nx},${ny}`);
          return hasLiberty(nx, ny);
        }
      }

      return false;
    };

    return !hasLiberty(x, y);
  }

  /** Logs a formatted {@link GobanMatrix} to the console. */
  prettyPrint() {
    console.log(this.goban.map((row) => [...row].join(" ")).join("\n"));
  }
}
