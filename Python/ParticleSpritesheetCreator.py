import pygame
import math
import time

pygame.init()

imagesFound = []
fileFinder = 1
while True:
    try:
        tempImage = pygame.image.load("C:/Users/harry/OneDrive/Documents/Unity/Complete Destruction/Python/SPR ({}).png".format(str(fileFinder)))
        print("Found SPR ({}).png".format(fileFinder))

        imagesFound.append(tempImage)
        fileFinder += 1
    except:
        print("Didn't find SPR ({}).png, so continuing with {}".format(str(fileFinder), str(fileFinder-1)))        
        break
    
maxSize = [0, 0]
rowCol = [0, 0]
fileFinder -= 1

for i in imagesFound:
    if i.get_width() > maxSize[0]:
        maxSize[0] = i.get_width()
        
    if i.get_height() > maxSize[1]:
        maxSize[1] = i.get_height()

print("Size of the biggest image is {},{}\n".format(str(maxSize[0]), str(maxSize[1])))


r = math.floor(math.sqrt(fileFinder))
c = r

while r*c < fileFinder:
    r += 1
    
rowCol[0] = round(r)
rowCol[1] = round(c)

    
print("Decided dimensions = {} rows, {} columns".format(str(r),str(c)))
time.sleep(1)


display = pygame.display.set_mode((maxSize[0] * rowCol[0], maxSize[1] * rowCol[1]))
pygame.display.set_caption("SPRITESHEET")
print("Created pygame display")

display.fill((0, 255, 255))
print("Filled pygame display with cyan: #00FFFF")

breakBoth = False
for y in range(rowCol[1]):
    for x in range(rowCol[0]):  

        try:
            currentImage = imagesFound[x + y * rowCol[0]]
        except:
            breakBoth = True
            break


        upperCorner = (x * maxSize[0], y * maxSize[1])
        offset = (math.floor((maxSize[0] - currentImage.get_width()) / 2), math.floor((maxSize[1] - currentImage.get_height()) / 2))

        display.blit(currentImage, (upperCorner[0] + offset[0], upperCorner[1] + offset[1]))


    if breakBoth:
        break








pygame.display.update()
pygame.image.save(display, "C:/Users/harry/OneDrive/Documents/Unity/Complete Destruction/Python/SPRSHEET.png")
print("Success!\nSprite sheet saved in: SPRSHEET.png")
time.sleep(1)



