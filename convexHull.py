import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

df = pd.read_csv("text.csv", names = ['x', 'y'])

df.x = df.x.str.extract('(\d+)', expand=False).astype(float)
df.y = df.y.str.extract('(\d+)', expand=False).astype(float)

graham_df, jarvis_df, chans_df, points_df = np.split(df, df[df.isnull().any(1)].index)
# because of empty space added when making output file
points_df = points_df.iloc[1:]
jarvis_df = jarvis_df.iloc[1:]
chans_df = chans_df.iloc[1:]
graham_df = pd.concat([graham_df, graham_df.head(1)], ignore_index=True)
jarvis_df = pd.concat([jarvis_df, jarvis_df.head(1)], ignore_index=True)
chans_df = pd.concat([chans_df, chans_df.head(1)], ignore_index=True)

print(jarvis_df.head(20))

ax1 = points_df.plot.scatter(x='x', y='y', c='DarkBlue', s=20, zorder = 1, label='points')
ax2 = graham_df.plot.line(x='x', y='y', style='-', c = 'pink', label='graham algo', linewidth=4, zorder= 2, ax=ax1)
ax3 = jarvis_df.plot.line(x='x', y='y', style='-', c = 'green', label='jarvis algo', linewidth=1, zorder = 3,  ax=ax1)
ax3 = chans_df.plot.line(x='x', y='y', style='-', c = 'black', label='chans_algo', linewidth=8, alpha = .5, zorder = 4, ax=ax1)

plt.legend()
plt.show()
