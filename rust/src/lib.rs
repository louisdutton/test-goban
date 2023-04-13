use std::collections::HashSet;

#[derive(Debug, PartialEq, Eq, Hash, Clone, Copy)]
enum Status {
    White = 1,
    Black = 2,
    Empty = 3,
    Out = 4,
}

struct Vec2 {
    x: i32,
    y: i32,
}

impl Vec2 {
    fn new(x: i32, y: i32) -> Vec2 {
        Vec2 { x, y }
    }
}

pub struct Goban {
    cells: Vec<Status>,
    width: usize,
    height: usize,
}

impl Goban {
    pub fn new(rows: Vec<&str>) -> Goban {
        let width = rows[0].len();
        let height = rows.len();
        let mut cells = Vec::with_capacity(width * height);

        for row in rows {
            for cell in row.chars() {
                match cell {
                    '.' => cells.push(Status::Empty),
                    '#' => cells.push(Status::Black),
                    'o' => cells.push(Status::White),
                    _ => panic!("Invalid character in board"),
                }
            }
        }

        return Goban {
            cells,
            width,
            height,
        };
    }

    fn in_bounds(&self, x: i32, y: i32) -> bool {
        return x >= 0 && x < self.width as i32 && y >= 0 && y < self.height as i32;
    }

    fn get_index(&self, x: i32, y: i32) -> usize {
        return (y * self.width as i32 + x) as usize;
    }

    fn get_status(&self, x: i32, y: i32) -> Status {
        if !self.in_bounds(x, y) {
            return Status::Out;
        } else {
            return self.cells[self.get_index(x, y)];
        }
    }

    fn has_liberty(&self, origin: Vec2, visited: &mut HashSet<usize>) -> bool {
        let origin_status = self.get_status(origin.x, origin.y);
        let mut neighbor_status;
        let mut next_position = Vec2::new(0, 0);
        let mut found_next = false;

        let neighbors = [
            Vec2::new(origin.x, origin.y - 1),
            Vec2::new(origin.x, origin.y + 1),
            Vec2::new(origin.x - 1, origin.y),
            Vec2::new(origin.x + 1, origin.y),
        ];

        println!("new node");

        for position in neighbors {
            let index = self.get_index(position.x, position.y);
            if visited.contains(&index) {
                continue;
            }

            neighbor_status = self.get_status(position.x, position.y);

            if neighbor_status == Status::Empty {
                return true;
            }
            if neighbor_status == origin_status {
                visited.insert(index);
                next_position = position;
                found_next = true;
            }
        }

        if found_next {
            return self.has_liberty(next_position, visited);
        }

        return false;
    }

    pub fn is_taken(&self, x: i32, y: i32) -> bool {
        let cell_count = self.cells.len();
        let mut visited = HashSet::with_capacity(cell_count);

        return !self.has_liberty(Vec2::new(x, y), &mut visited);
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn white_taken_when_surrounded() {
        let rows = vec![".#.", "#o#", ".#."];
        let goban = Goban::new(rows);
        assert_eq!(goban.is_taken(1, 1), true);
    }

    #[test]
    fn white_not_taken_with_liberty() {
        let rows = vec!["...", "#o#", ".#."];
        let goban = Goban::new(rows);
        assert_eq!(goban.is_taken(1, 2), false);
    }

    #[test]
    fn black_shape_taken_when_surrounded() {
        let rows = vec!["oo.", "##o", "o#o", ".o."];
        let goban = Goban::new(rows);
        assert_eq!(goban.is_taken(0, 1), true);
        assert_eq!(goban.is_taken(1, 1), true);
        assert_eq!(goban.is_taken(1, 2), true);
    }

    #[test]
    fn black_shape_not_taken_with_liberty() {
        let rows = vec!["oo.", "##.", "o#o", ".o."];
        let goban = Goban::new(rows);
        assert_eq!(goban.is_taken(0, 1), false);
        assert_eq!(goban.is_taken(1, 1), false);
        assert_eq!(goban.is_taken(1, 2), false);
    }

    #[test]
    fn square_shape_taken() {
        let rows = vec!["oo.", "##o", "##o", "oo."];
        let goban = Goban::new(rows);

        assert_eq!(goban.is_taken(0, 1), true);
        assert_eq!(goban.is_taken(0, 2), true);
        assert_eq!(goban.is_taken(1, 1), true);
        assert_eq!(goban.is_taken(1, 2), true);
    }
}
