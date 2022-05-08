        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="xData">横坐标集合</param>
        /// <param name="YData">纵坐标集合</param>
        /// <param name="title">标题</param>
        private void DrawGraph(List<double> xData, List<double> YData, string title)
        {
            //画线
            ZedDraw.GraphPane.CurveList.Clear();
            ZedDraw.GraphPane.Legend.IsVisible = false;
            ZedDraw.GraphPane.Legend.Border.IsVisible = false;
            ZedDraw.GraphPane.YAxis.MajorGrid.IsZeroLine = true;
            ZedDraw.GraphPane.XAxis.Scale.Format = "0.000";
            ZedDraw.GraphPane.YAxis.Scale.Format = "0.0";
            ZedDraw.Refresh();
            //创建点集合对象
            PointPairList NormalPoints = new PointPairList();
            LineItem NormalLine = ZedDraw.GraphPane.AddCurve("光谱曲线", NormalPoints, Color.Black, SymbolType.None);
            NormalLine.IsVisible = true;
            NormalLine.Line.IsVisible = true;
            double yMax = 0;
            ZedDraw.GraphPane.YAxis.Title.Text = "强度";
            ZedDraw.GraphPane.XAxis.Title.Text = "波长(nm)";
            ZedDraw.GraphPane.Title.Text = title;
            for (int i = 0; i < xData.Count; i++)//普通曲线是必绘制的
            {
                NormalPoints.Add(xData[i], YData[i]);
                if (YData[i] > yMax)
                {
                    yMax = YData[i];
                }
            }
            ZedDraw.GraphPane.YAxis.Scale.Min = 0;
            ZedDraw.GraphPane.YAxis.Scale.Max = yMax * 1.1;
            ZedDraw.GraphPane.XAxis.Scale.Min = xData[0];
            ZedDraw.GraphPane.XAxis.Scale.Max = xData[xData.Count - 1];
            ZedDraw.AxisChange();
            ZedDraw.Refresh();
        }
