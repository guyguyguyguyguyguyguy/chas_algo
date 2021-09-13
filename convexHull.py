import pandas as pd
import matplotlib.pyplot as plt
import matplotlib.colors as mcol
import numpy as np
import random

all_colours = list(mcol.CSS4_COLORS.keys())
random.shuffle(all_colours)

df = pd.read_csv("text.csv", names = ['x', 'y'])

df.x = df.x.str.extract('(\d+)', expand=False).astype(float)
df.y = df.y.str.extract('(\d+)', expand=False).astype(float)

# graham_df, jarvis_df, mini_df, chans_df, points_df = np.split(df, df[df.isnull().any(1)].index)
# # because of empty space added when making output file
# points_df = points_df.iloc[1:]
# jarvis_df = jarvis_df.iloc[1:]
# chans_df = chans_df.iloc[1:]
# mini_df = mini_df.iloc[1:]
# graham_df = pd.concat([graham_df, graham_df.head(1)], ignore_index=True)
# jarvis_df = pd.concat([jarvis_df, jarvis_df.head(1)], ignore_index=True)
# chans_df = pd.concat([chans_df, chans_df.head(1)], ignore_index=True)
# mini_df = pd.concat([mini_df, mini_df.head(1)], ignore_index=True)

dfs = np.split(df, df[df.isnull().any(1)].index)

for i in range(1, len(dfs)):
    dfs[i] = dfs[i].iloc[1:]

    if i != len(dfs):
        dfs[i] = pd.concat([dfs[i], dfs[i].head(1)], ignore_index=True)

ax1 = dfs[-1].plot.scatter(x='x', y='y', c='DarkBlue', s=20, zorder = 1, label='points')
ax4 = dfs[-2].plot.scatter(x='x', y='y', c="orange", s = 100, zorder = 0, alpha = .5, label='mini', ax=ax1)
# ax2 = dfs[1].plot.line(x='x', y='y', style='-', c = 'gold', label='graham algo', linewidth=2, zorder= 2, ax=ax1)
# ax3 = dfs[0].plot.line(x='x', y='y', style='-', c = 'green', label='jarvis algo', linewidth=1, zorder = 3,  ax=ax1)
ax3 = dfs[-3].plot.line(x='x', y='y', style='-', c = 'black', label='chan algo', linewidth=4, alpha =.5, zorder = 3,  ax=ax1)

for i in range(2, len(dfs) -3):
    ax = dfs[i].plot.line(x='x', y='y', style='-', c = all_colours[i], linewidth=4, alpha =0.3, zorder= 2, label='_nolegend_', ax=ax1)

# ax1 = points_df.plot.scatter(x='x', y='y', c='DarkBlue', s=20, zorder = 1, label='points')
# ax2 = graham_df.plot.line(x='x', y='y', style='-', c = 'pink', label='graham algo', linewidth=4, zorder= 2, ax=ax1)
# ax3 = jarvis_df.plot.line(x='x', y='y', style='-', c = 'green', label='jarvis algo', linewidth=1, zorder = 3,  ax=ax1)
# ax3 = chans_df.plot.line(x='x', y='y', style='-', c = 'black', label='chans_algo', linewidth=8, alpha = .5, zorder = 4, ax=ax1)
# ax4 = mini_df.plot.scatter(x='x', y='y', c="orange", s = 100, zorder = 0, alpha = .5, label='mini', ax=ax1)

print(dfs[-3])

plt.legend()
plt.show()
