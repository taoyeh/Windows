# Windows# 前言
> 这次的作业逻辑略微有点混乱，后面才稍微有点清楚

# 完成功能

- 提供对矩形/椭圆/直线/曲线的绘制，具有类似“橡皮筋”的视觉效果

- 提供选项，允许用户对这些图形的颜色、线型、可填充性等属性进行选择

- 抽象出一个图形基类，对每一种具体的图形类型进行独立编程，提供了更好的可扩展性

- 实现重绘、永久保存/从文件加载图形集合，保存为图片等功能

- 具有对图像进行擦除/复制/粘贴/剪切/撤销等基本操作

![AYT_E_T7B_UEV_OVMS_0_~R.png](https://i.loli.net/2020/05/13/kS6cNHWAenEPIZT.png)

# 实现思路
### 双缓冲技术
&emsp;&emsp;导致画面闪烁的关键原因是：窗口刷新一次的过程中，每一个图元的重绘都会立即显示到窗口，因此整个窗口中，只要是图元所在的位置，都在刷新，而刷新的时间是有差别的，闪烁现象自然会出现。所以说，此时导致窗口闪烁现象的关键因素并不在于Paint事件调用的次数多少，而在于各个图元的重绘。使用主画布和次画布，用户看不到次画布，闪烁等问题都留在次画布，最终完成之后再映射到主画布。
### 使用枚举和抽出基类
- 使用枚举代码规范，可读性高
```
//枚举类型，各种绘图工具
        public  enum drawTools
        {
             Pen=0,Line, Ellipse, Rectangle, Rubber, None
        };
```
- 抽出基类操作管理方便，简单高效
```
 public class Shape
    {
        //图形类型
        public drawTools DrawTool;

        public Pen pen;

        public Point start, end;

        public bool Fill;

        public Color FillColor;
        //一系列的点是为了记录曲线
        public List<Point> Points;
        //构造函数
        //显示函数

    }
```
### 利用MouseDown，MouseUp,MouseMove实现绘图
&emsp;&emsp;总体思路就是MouseDown的时候初始化画布，MouseMove利用双缓冲来实现画布上的画图，MouseUp的时候将画的图形以数据的保存起来。复制，粘贴，剪切就是在MouseMove或者MouseUp的时候利用当前点找到所属图形，之后进行一系列操作。


### 源码
  [传送门](https://github.com/taoyeh/Windows/tree/GDI)

