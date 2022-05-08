  /*
  OriginalData是导入的未扣背景数据
  */
        public List<int> IndexOfMin = new List<int>();//最终筛选的极小值在原y数据的索引
        public static List<double> UseYdata = new List<double>();//经过最后筛选的极小值点
        public static List<double> UseXdata = new List<double>();//筛选极小值的x坐标
        
        /// <summary>
        /// 双极小值扣背景
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutBackground_Click(object sender, EventArgs e)
        {
            GetIndexAndData();
            DelBackgroud();
            DrawGraph(OriginalDataX, OriginalDataY, "扣除背景谱图");
        }
        
        
        /// <summary>
        /// 取得波谷(极小值的下标)
        /// </summary>
        /// <param name="diff"></param>
        /// <returns></returns>
        private List<int> GetIndex(int[] diff)
        {
            List<int> index = new List<int>();
            for (int i = 0; i != diff.Length - 1; i++)
            {
                if (diff[i + 1] - diff[i] == 2)
                {
                    index.Add(i + 1);
                }
            }
            return index;
        }
        
        /// <summary>
        /// 计算Ydata的一阶差分和符号函数
        /// </summary>
        /// <returns>符号函数</returns>
        private int[] GetDiff(List<double> DataList)
        {
            int[] DiffY = new int[DataList.Count - 1];
            for (int i = 0; i < DiffY.Length; i++)
            {
                if (DataList[i + 1] - DataList[i] > 0)
                {
                    DiffY[i] = 1;
                }
                else if (DataList[i + 1] - DataList[i] < 0)
                {
                    DiffY[i] = -1;
                }
                else
                {
                    DiffY[i] = 0;
                }
            }
            for (int i = DiffY.Length - 1; i >= 0; i--)
            {
                if (DiffY[i] == 0 && i == DiffY.Length - 1)
                {
                    DiffY[i] = 1;
                }
                else if (DiffY[i] == 0)
                {
                    if (DiffY[i + 1] >= 0)
                    {
                        DiffY[i] = 1;
                    }
                    else
                    {
                        DiffY[i] = -1;
                    }
                }
            }
            return DiffY;
        }
        
        /// <summary>
        /// 计算参数指标
        /// </summary>
        /// <param name="dataArry"></param>
        /// <param name="vi"></param>
        /// <param name="xt"></param>
        private void GetViandXita(double[] dataArry, out double[] vi, out double xt)
        {
            vi = new double[dataArry.Length];
            double ave, sum = 0, vix = 0, n;
            //求平均值
            for (int i = 0; i < dataArry.Length; i++)
            {
                sum += dataArry[i];
            }
            ave = sum / dataArry.Length;
            //求残余误差
            for (int i = 0; i < dataArry.Length; i++)
            {
                vi[i] = dataArry[i] - ave;
            }
            //求标准偏差
            for (int i = 0; i < vi.Length; i++)
            {
                vix += Math.Pow(vi[i], 2);
            }
            n = (vi.Length - 1) * 1.0;
            xt = Math.Sqrt(vix / n);
        }
        
           private void GetIndexAndData()
        {
            Dictionary<int, double> cutData = new Dictionary<int, double>();
            List<int> firMinIndex = new List<int>();
            List<int> secMinIndex = new List<int>();
            List<double> firYdata = new List<double>();
            List<int> tichusuoyin = new List<int>();
            double[] vi = null;
            double xt;
            //第一次筛选
            firMinIndex = GetIndex(GetDiff(OriginalDataY));//第一次筛选的极小值在原始数据中的位置
            foreach (int p in firMinIndex)
            {
                firYdata.Add(OriginalDataY[p]);
            }
            //第二次筛选
            secMinIndex = GetIndex(GetDiff(firYdata));
            foreach (int p in secMinIndex)
            {
                //cutData.Add(firMinIndex[p], firYdata[p]);//第二次筛选的最小值的索引和强度的键值对(根据索引找值)
                IndexOfMin.Add(firMinIndex[p]);
                UseYdata.Add(firYdata[p]);
            }
            int windowsNumber = 30;
            int dataNumber = UseYdata.Count / 30;
            double[] dataArry = new double[dataNumber];
            for (int i = 0; i < windowsNumber; i++)
            {
                UseYdata.CopyTo(i * dataNumber, dataArry, 0, dataNumber);//在数组中的索引为在原集合中的索引
                GetViandXita(dataArry, out vi, out xt);
                for (int j = 0; j < vi.Length; j++)
                {
                    if (vi[j] > 3 * xt)
                    {
                        tichusuoyin.Add(i * dataNumber + j);//存储需要剔除的值的索引
                    }
                }
            }
            //剔除数据开始
            foreach (int p in tichusuoyin)
            {
                IndexOfMin.RemoveAt(p);
                UseYdata.RemoveAt(p);
            }
            //剔除完之后UseYdata为最终的极小值，IndexOfMin为最终极小值的索引
            foreach (int p in IndexOfMin)
            {
                UseXdata.Add(OriginalDataX[p]);
            }
        }
        
        /// <summary>
        /// 扣除背景
        /// </summary>
        private void DelBackgroud()
        {
            //直接法扣背景
            int temp = 0, k = 0;
            foreach (int p in IndexOfMin)
            {
                for (int i = temp; i < p; i++)
                {
                    OriginalDataY[i] = OriginalDataY[i] - UseYdata[k];
                    if (OriginalDataY[i] < 0)
                    {
                        OriginalDataY[i] = 0;
                    }
                }
                k = k + 1;
                temp = p;
            }
        }
