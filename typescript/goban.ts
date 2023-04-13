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
  goban: GobanMatrix;
  dimensions: [number, number];

  constructor(goban: GobanMatrix) {
    this.goban = goban;
    this.dimensions = [this.goban.length, this.goban[0].length];
  }

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

  isTaken(x: number, y: number) {
    const checked = new Set<number>();
    const nextPosition = [0, 0];
    const neighbors = [
      [0, 0],
      [0, 0],
      [0, 0],
      [0, 0],
    ];

    const hasLiberty = (x: number, y: number) => {
      const stone = this.getStatus(x, y);
      let foundNext = false;

      // up
      neighbors[3][0] = x;
      neighbors[3][1] = y - 1;

      // down
      neighbors[2][0] = x;
      neighbors[2][1] = y + 1;

      // right
      neighbors[0][0] = x + 1;
      neighbors[0][1] = y;

      // left
      neighbors[1][0] = x - 1;
      neighbors[1][1] = y;

      for (const [nx, ny] of neighbors) {
        const index = this.getIndex(nx, ny);
        if (checked.has(index)) continue;

        const status = this.getStatus(nx, ny);

        if (status === Status.Empty) return true;
        if (status === stone) {
          checked.add(index);
          nextPosition[0] = nx;
          nextPosition[1] = ny;
          foundNext = true;
        }
      }

      // prettier-ignore
      return foundNext
        ? hasLiberty(nextPosition[0], nextPosition[1])
        : false;
    };

    return !hasLiberty(x, y);
  }

  getIndex(x: number, y: number) {
    return y * this.dimensions[0] + x;
  }
}
