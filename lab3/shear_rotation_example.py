from pprint import pprint

rows_count = 7
cols_count = 7

def rotate(matrix, center, angle=45, verbose=False):
    import math

    shear =-0.1
    return 0

def xshear(img, shear, width, height):
    import math
    xc = int(width/2)
    yc = int(height/2)

    for y in range(0, height):
        yt = y - yc
        delta = round(shear*yt)
        if delta < 0:
            for x in range(0, width):
                if x-delta>=width:
                    img[y][x] = ' '
                else:
                    img[y][x] = img[y][x-delta]
        elif delta >= 0:
            for x in reversed(range(0, width)):
                if x-delta<0:
                    img[y][x] = ' '
                else:
                    img[y][x] = img[y][x-delta]

def yshear(img, shear, width, height):
    import math
    xc = int(width/2)
    yc = int(height/2)

    for x in range(0,width):
        xt = x - xc
        delta = round(shear*xt)
        if delta < 0:
            for y in range(0, height):
                if y-delta>=height:
                    img[y][x] = ' '
                else:
                    img[y][x] = img[y-delta][x]
        elif delta >= 0:
            for y in reversed(range(0, height)):
                if y-delta<0:
                    img[y][x] = ' '
                else:
                    img[y][x] = img[y-delta][x]

def shear_rotate(img, width, height, angle):
    import math
    rad = math.radians(angle)
    a = -math.tan(rad/2)
    b = math.sin(rad)

    xshear(img, a, width, height)
    yshear(img, b, width, height)
    xshear(img, a, width, height)


if __name__ == '__main__':
    img = [[' ' for x in range(cols_count)] for x in range(rows_count)]
    row = 0
    col = 0
    for i in range(1,10):
        img[row+2][col+2] = str(i)
        col += 1
        if col+1 == 4:
            col = 0
            row +=1

    img[1][3] = '3'
    img[5][3] = '3'

    pprint(img)
    print(25*'-')
    shear_rotate(img, rows_count, cols_count, 75)
    pprint(img)
