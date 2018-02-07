import itertools
import math
from pprint import pprint

OCTAHEDRON = [
    [(0.0, -1.0, 0.0), (1.0, 0.0, 0.0), (0.0, 0.0, 1.0)],
    [(0.0, -1.0, 0.0), (0.0, 0.0, 1.0), (-1.0, 0.0, 0.0)],
    [(0.0, -1.0, 0.0), (-1.0, 0.0, 0.0), (0.0, 0.0, -1.0)],
    [(0.0, -1.0, 0.0), (0.0, 0.0, -1.0), (1.0, 0.0, 0.0)],
    [(1.0, 0.0, 0.0), (0.0, 1.0, 0.0), (0.0, 0.0, 1.0)],
    [(0.0, 0.0, 1.0), (0.0, 1.0, 0.0), (-1.0, 0.0, 0.0)],
    [(-1.0, 0.0, 0.0), (0.0, 1.0, 0.0), (0.0, 0.0, -1.0)],
    [(0.0, 0.0, -1.0), (0.0, 1.0, 0.0), (1.0, 0.0, 0.0)]
]

def sm(tup1, tup2):
    import operator
    return tuple(map(operator.add, tup1, tup2))

def divide_face(triangle, curr_lvl):
    if curr_lvl <= 0:
        return list(triangle)

    t01 = tuple(ti/2 for ti in sm(triangle[0], triangle[1]))
    t12 = tuple(ti/2 for ti in sm(triangle[1], triangle[2]))
    t02 = tuple(ti/2 for ti in sm(triangle[0], triangle[2]))

    smaller = [
    [triangle[0], t01, t02],
    [t01, triangle[1], t12],
    [t02, t12, triangle[2]],
    [t01, t12, t02]
    ]
    result = []

    good_smaller = []
    for small in smaller:
        good_points = []
        for p in small:
            s = math.sqrt(p[0] ** 2 + p[1] ** 2 + p[2] ** 2)
            good_points.append((p[0]/s, p[1]/s, p[2]/s))
        good_smaller.append(good_points)

    for small in good_smaller:
        for smallest in divide_face(small, curr_lvl-1):
            result.append(smallest)
    return result

def print_nicely(result):
    for point in result:
        txt = ''
        for _ in range(2):
            for coord in point:
                txt += str(coord) + ', '
        print(txt)
            

def main():
    result = []
    for face in OCTAHEDRON:
        for partial in divide_face(face, 3):
            result.append(partial)
    print_nicely(result)

if __name__ == '__main__':
    main()