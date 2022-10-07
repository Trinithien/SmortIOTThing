import numpy as np
import matplotlib.pyplot as plt
fig = plt.figure(figsize =(10, 10))
ax = fig.add_subplot()
t = np.linspace(-5, 5, 100)
x = t  
y = t**2 #Since (x,y) go goes thru the point (-1,1), (0,0) and (1,1) 
#the formula for y will by y**2.
ax.plot(x, y, label="A parametric curve")
ax.legend()
plt.show()
