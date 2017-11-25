from pprint import pprint

rows_count = 5
cols_count = 5
    
def rotate(matrix, center, angle=45, verbose=False):
    import math
    rad = math.radians(angle)
    cos = math.cos(rad)
    sin = math.sin(rad)
    rotated = [[0 for x in range(cols_count)] for x in range(rows_count)]
    for x in range(rows_count):
        for y in range(cols_count):
            xc, yc = center
            xt = x - xc
            yt = y - yc
            xr = xt * cos + yt * sin
            yr = -xt * sin + yt * cos
            x2 = xr + xc
            y2 = yr + yc
            x2 = round(x2)
            y2 = round(y2)
            if(verbose):
                print('({}, {}) <- ({}, {})'.format(x, y, x2, y2))
            if x2<0 or x2>=cols_count or y2<0 or y2>=rows_count:
                rotated[x][y] = 0
            else:
                rotated[x][y] = original[x2][y2]
    return rotated

if __name__ == '__main__':
    original = [[0 for x in range(cols_count)] for x in range(rows_count)]
    row = 1
    col = 1
    for i in range(1,10):
        original[row][col] = i
        col += 1
        if col == 4:
            col = 1
            row +=1

    pprint(original)
    print(25*'-')
    pprint(rotate(original, (2,2), 45))