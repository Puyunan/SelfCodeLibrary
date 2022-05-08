        private void ZedDraw_MouseMove(object sender, MouseEventArgs e)
        {
            PointF mousePt = new PointF(e.X, e.Y);
            string tooltip = string.Empty;
            GraphPane pane = ((ZedGraphControl)sender).MasterPane.FindChartRect(mousePt);
            if (pane != null)
            {
                double x, y;
                // 转换鼠标坐标为X和Y的尺度
                pane.ReverseTransform(mousePt, out x, out y);
                // 获取横纵坐标信息
                tooltip = "(" + x.ToString("f2") + ", " + y.ToString("f2") + ")";
            }
            TipXandY.SetToolTip(ZedDraw, tooltip);
        }
