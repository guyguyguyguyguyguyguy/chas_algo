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

dfs = np.split(df, df[df.isnull().any(1)].index)

for i in range(1, len(dfs)):
    dfs[i] = dfs[i].iloc[1:]

    if i != len(dfs):
        dfs[i] = pd.concat([dfs[i], dfs[i].head(1)], ignore_index=True)

ax1 = dfs[-1].plot.scatter(x='x', y='y', c='DarkBlue', s=.5, zorder = 1, label='points')
ax4 = dfs[-2].plot.scatter(x='x', y='y', c="orange", s = 5, zorder = 0, alpha = .5, label='mini', ax=ax1)
ax2 = dfs[1].plot.line(x='x', y='y', style='-', c = 'gold', label='graham algo', linewidth=2, zorder= 2, ax=ax1)
# ax3 = dfs[0].plot.line(x='x', y='y', style='-', c = 'green', label='jarvis algo', linewidth=1, zorder = 3,  ax=ax1)
ax3 = dfs[-3].plot.line(x='x', y='y', style='-', c = 'black', label='chan algo', linewidth=4, alpha =.5, zorder = 3,  ax=ax1)

 # for i in range(2, len(dfs) -3):
 #    ax = dfs[i].plot.line(x='x', y='y', style='-', c = all_colours[i], linewidth=4, alpha =0.3, zorder= 2, label='_nolegend_', ax=ax1)

print(dfs[-3].head(len(dfs[-3])))

plt.legend()
plt.show()
