import enum


class Status(enum.Enum):
    """
    Enum representing the Status of a position on a goban
    """

    WHITE = 1
    BLACK = 2
    EMPTY = 3
    OUT = 4


class Goban(object):
    def __init__(self, goban):
        self.goban = goban

    def get_status(self, x: int, y: int) -> Status:
        """
        Get the status of a given position

        Args:
            x: the x coordinate
            y: the y coordinate

        Returns:
            a Status
        """

        if (
            not self.goban
            or x < 0
            or y < 0
            or y >= len(self.goban)
            or x >= len(self.goban[0])
        ):
            return Status.OUT
        elif self.goban[y][x] == ".":
            return Status.EMPTY
        elif self.goban[y][x] == "o":
            return Status.WHITE
        elif self.goban[y][x] == "#":
            return Status.BLACK

    def is_taken(self, x: int, y: int) -> bool:
        checked = set()

        def has_liberty(x: int, y: int) -> bool:
            stone = self.get_status(x, y)
            nextPosition: tuple = None
            neighbors = (
                (x + 1, y),
                (x - 1, y),
                (x, y + 1),
                (x, y - 1),
            )

            for neighbor in neighbors:
                if neighbor in checked:
                    continue

                status = self.get_status(neighbor[0], neighbor[1])
                checked.add(neighbor)

                if status == Status.EMPTY:
                    return True
                if (status == stone):
                    checked.add((x, y))
                    nextPosition = neighbor

            if nextPosition != None:
                return has_liberty(nextPosition[0], nextPosition[1])

            return False

        return not has_liberty(x, y)
