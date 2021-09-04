import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

df = pd.read_csv("text.csv", names = ['x', 'y'])

df.x = df.x.str.extract('(\d+)', expand=False).astype(float)
df.y = df.y.str.extract('(\d+)', expand=False).astype(float)

graham_df, jarvis_df, points_df = np.split(df, df[df.isnull().any(1)].index)
points_df = points_df.iloc[1:]
jarvis_df = jarvis_df.iloc[1:]
graham_df = pd.concat([graham_df, graham_df.head(1)], ignore_index=True)
jarvis_df = pd.concat([jarvis_df, jarvis_df.head(1)], ignore_index=True)

print(jarvis_df.head(20))

ax1 = points_df.plot.scatter(x='x', y='y', c='DarkBlue', s=20, label='points')
ax2 = graham_df.plot.line(x='x', y='y', style='-', c = 'pink', label='convex hull', linewidth=4, ax=ax1)
ax3 = jarvis_df.plot.line(x='x', y='y', style='-', c = 'green', label='convex hull', linewidth=1, ax=ax1)

plt.show()
