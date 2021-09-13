using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

// split each algorithm into its own class for cleanliness
// QuickHull for higher dimensional data


class Chan
{
  [Serializable]
  struct Point
  {
    public int x, y;

    public Point(int a, int b)
    {
      x = a;
      y = b;
    }

    public static bool operator ==(Point p1, Point p2)
    {
      return p1.Equals(p2);
    }

    public static bool operator !=(Point p1, Point p2)
    {
      return !p1.Equals(p2);
    }

    public override string ToString()
    {
      return "Point: " + x + ", " + y; 
    }
  }


  public class YPointComparison : IComparer<Point>
  {
    int IComparer<Point>.Compare( Point x, Point y )
    {
      if (x.y != y.y) return x.y.CompareTo(y.y);
      return x.x.CompareTo(y.x);
    }
  }

  public class XPointComparison : IComparer<Point>
  {
    int IComparer<Point>.Compare( Point x, Point y )
    {
      if (x.x != y.x) return x.x.CompareTo(y.x);
      return x.y.CompareTo(y.y);
    }
  }

  static void Main(string[] args)
  {
    Random randNum = new Random();
    int Min = 0;
    int Max = 100;
    Point[] points = Enumerable
      .Repeat(0, 100)
      .Select(i => new Point(randNum.Next(Min, Max), randNum.Next(Min, Max)))
      .ToArray();

    Point[] jarvisConvexHull = JarvisMarch(points, points.Length).ToArray();
    Point[] grahamConvexHull = GrahamScan(points).ToArray();
    (List<List<Point>> miniConvexHulls, Point[] miniConvexHull, Point[] chanConvexHull) = optimisedChansAlgo(points, 7);

    using (System.IO.StreamWriter file = new System.IO.StreamWriter("text.csv"))
    {
          file.Write(string.Join('\n', grahamConvexHull.Select(p => (p.x, p.y))));
          file.Write("\n , \n");
          file.Write(string.Join('\n', jarvisConvexHull.Select(p => (p.x, p.y))));
          file.Write("\n , \n");
          foreach(List<Point> ps in miniConvexHulls)
          {
            file.Write(string.Join('\n', ps.ToArray().Select(p => (p.x, p.y))));
            file.Write("\n, \n");
          }
          file.Write(string.Join('\n', chanConvexHull.Select(p => (p.x, p.y))));
          file.Write("\n , \n");
          file.Write(string.Join('\n', miniConvexHull.Select(p => (p.x, p.y))));
          file.Write("\n , \n");
          file.Write(string.Join('\n', points.Select(p => (p.x, p.y))));
    }
    
    // float convexHullArea = AreaFunc(convexHull);
  }

  // sort points by x value -> smallest to largest
  private static void sortLeftOrBottomMostPoint(ref Point[] points, string axis)
  {
    if (axis == "x") {
      // Array.Sort(points, (p1, p2) => p1.x.CompareTo(p2.x));
      Array.Sort(points, new XPointComparison());
    } else if (axis == "y")
    {
      // Array.Sort(points, (p1, p2) => p1.y.CompareTo(p2.y));
      Array.Sort(points, new YPointComparison());
    }
  }

  private static bool Ccw(Point a, Point b, Point c)
  {
    return ((b.x - a.x) * (c.y - a.y)) > ((b.y - a.y) * (c.x - a.x));
  }

  // Method of finding convex hull of finite set of points in the plane -> O(n log n)
  private static List<Point> GrahamScan(Point[] points)
  {
    List<Point> convexHull = new List<Point>();
  
    if (points.Length < 4)
    {
      convexHull.AddRange(points);
      return convexHull;
    }

    sortLeftOrBottomMostPoint(ref points, "x");

    foreach (Point pt in points)
    {
      while (convexHull.Count >= 2 && !Ccw(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], pt)) {
        convexHull.RemoveAt(convexHull.Count - 1);
      }
      convexHull.Add(pt);
    }

    int lowerHull = convexHull.Count + 1;
    for (int i = points.Length - 1; i >= 0; --i)
    {
      Point pt = points[i];
      while (convexHull.Count >= lowerHull && !Ccw(convexHull[convexHull.Count - 2], convexHull[convexHull.Count -1], pt)) {
        convexHull.RemoveAt(convexHull.Count - 1);
      }
      convexHull.Add(pt);
    }

    convexHull.RemoveAt(convexHull.Count - 1);
    return convexHull;
  }

  private static int orientation(Point p, Point q, Point r)
  {
      int val = (q.y - p.y) * (r.x - q.x) -
              (q.x - p.x) * (r.y - q.y);

     if (val == 0) return 0; // collinear
     return (val > 0) ? 1 : 2; // clock or counterclock wise
  }

  private static List<Point> JarvisMarch(Point[] points, int n)
  {
      if (n < 3) return new List<Point>();

      List<Point> hull = new List<Point>();

      int l = 0;
      for (int i = 1; i < n; i++)
          if (points[i].x < points[l].x)
              l = i;

      int p = l, q;
      do
      {
          hull.Add(points[p]);

          q = (p + 1) % n;

          for (int i = 0; i < n; i++)
          {

              if (orientation(points[p], points[i], points[q])
                                                  == 2)
                  q = i;
          }

          p = q;

      } while (p != l);

      return hull;
  }

  private static (double, int) orientationNew(Point p, Point q, Point r)
  {
    double val = Math.Atan2(q.y - p.y, q.x - p.x) - Math.Atan2(r.y - p.y, r.x - p.x);

    int ori = (q.y - p.y) * (r.x - q.x) -
            (q.x - p.x) * (r.y - q.y);

    if (ori == 0) return (val, 1); // collinear
    return (val > 1) ? (val, 1) : (val, 2); // clock or counterclock wise
  }

  private static Point AltJarvisMarch(Point point, Point[] points, Point prevPoint)
  {
    double angle = int.MinValue;
    Point nextP = new Point();

    Console.WriteLine("new jarvis march");

    foreach (Point p in points)
    {
      Console.WriteLine(p);
      // Cant turn corners which is a bit wack => So only get half of the convex hull
      (double newA, int val) = orientationNew(point, p, prevPoint);
      if (val == 1 && newA > angle)
      {
        angle = newA;
        nextP = p;
      }
    }

    return nextP;

    throw new ArgumentException("Orientation did not return 2");
  }

  private static (List<List<Point>>, Point[], List<Point>, bool) ChansAlgo(Point[] points, int m)
  {
    // int k = points.Length/m;
    List<Point> chunkConvexHull = new List<Point>();
    List<List<Point>> sepChunkConvexHull = new List<List<Point>>();
    Point[] chunk;

    if (m > points.Length)
    {
      m = points.Length;
    }

    Console.WriteLine("m is currently: " + m);

    for (int i = 0; i < points.Length; i+=m)
    {
      if (i + m < points.Length) {
        chunk = new Point[m];
        Array.Copy(points, i, chunk, 0, m);
      } else {
        int n = points.Length - i;
        chunk = new Point[n];
        Array.Copy(points, i, chunk, 0, n);
      }

      List<Point> convexChunk = GrahamScan(chunk);
      sepChunkConvexHull.Add(convexChunk);
      chunkConvexHull.AddRange(convexChunk);
    }


    Point[] miniConvexHull = chunkConvexHull.ToArray();
    Point[] leftPoints = (Point[])miniConvexHull.Clone();
    // List<Point> totalConvexHull = JarvisMarch(miniConvexHull, miniConvexHull.Length);


    List<Point> totalConvexHull = new List<Point>();
    Point currPoint = new Point(int.MaxValue, 0);
    foreach (Point p in chunkConvexHull) 
    {
      if (p.x < currPoint.x)
        currPoint = p;
    }

    Point prevPoint = new Point(int.MinValue, 0);
    totalConvexHull.Add(currPoint);
    // leftPoints = leftPoints.Except(new Point[]{currPoint}).ToArray();

    for (int i = 0; i < m; ++i)
    {
      // points = points.where(val => val != currPoint).ToArray();
      Point convexHullPoint = AltJarvisMarch(currPoint, leftPoints, prevPoint);
      prevPoint = currPoint;
      currPoint = convexHullPoint;
      totalConvexHull.Add(currPoint);
      leftPoints = leftPoints.Except(new Point[]{currPoint}).ToArray();
      if (currPoint == totalConvexHull[0]) {
        return (sepChunkConvexHull, miniConvexHull, totalConvexHull, true);
      } else {
        // totalConvexHull.Add(currPoint);
      }
    }

    if (m == points.Length)
    {
      return (sepChunkConvexHull, miniConvexHull, totalConvexHull, true);
    }

    return (sepChunkConvexHull, miniConvexHull, totalConvexHull, false);
  }

  private static (List<List<Point>>, Point[], Point[]) optimisedChansAlgo(Point[] points, int m)
  {
    List<Point> totalConvexHull = new List<Point>();
    Point[] miniConvexHull;
    List<List<Point>> miniConvexHulls = new List<List<Point>>();
    bool isDone = false;
    int n = points.Length;

    do {
      (miniConvexHulls, miniConvexHull, totalConvexHull, isDone) = ChansAlgo(points, m);
      m *= m;
    } while (!isDone);

    return (miniConvexHulls, miniConvexHull, totalConvexHull.ToArray());
  }

  private static float AreaFunc(Point[] points)
  {

    return 0.0f;
  }

  private static void SerialisePoints(Point[] points, Stream s)
  {
    IFormatter formatter = new BinaryFormatter();
    formatter.Serialize(s, points);
  }
}
