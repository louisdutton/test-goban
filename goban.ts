/** Enum representing the Status of a position on a goban */
const enum Status {
  White = 1,
  Black = 2,
  Empty = 3,
  Out = 4,
}

type StatusRepresentation = "." | "o" | "#";
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
   *  @argument x: the x coordinate
   *  @argument y: the x coordinate
   **/
  getStatus(x: number, y: number): Status {
    if (x < 0 || y < 0 || y >= this.goban.length || x >= this.goban[0].length) {
      return Status.Out;
    } else {
      return statusDictionary[this.goban[y][x]];
    }
  }

  /**
   *  Returns true if the position is taken.
   *  @argument x: the x coordinate
   *  @argument y: the x coordinate
   **/
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

  /** Logs a formatted goban matrix to the console. */
  prettyPrint() {
    console.log(this.goban.map((row) => [...row].join(" ")).join("\n"));
  }
}
