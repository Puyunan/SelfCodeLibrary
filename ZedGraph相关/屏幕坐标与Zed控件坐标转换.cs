//先是基础的直接点击获取屏幕坐标后转换为控件坐标
private void ZedGraph_MouseClick(object sender,MouseEventArgs e)
{
    PointF mousePt = new PointF(e.X,e.Y);//创建点击的坐标点，存储的是屏幕坐标
    GraphPane pane = ((ZedGraphControl)sender).MasterPane.FindChartRect(mousePt);
    if(pane!=null)
    {
        double x,y;
        pane.ReverseTransform(mousePt,out x,out y);//这里的x和y就是Zed中的坐标
    }
}
