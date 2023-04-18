public static (int[] peakPositions, Vector<double> yTransformed, Vector<double> T)
        FindPeaks(Vector<double> y, int windowWidth, double thresholdFactor, string lineshapeType, double H_G, double H_L, double k)
        {
            int m = (windowWidth - 1) / 2;
            var x = Vector<double>.Build.Dense(windowWidth, i => -m + i);
            int n = y.Count;
            Vector<double> f;
            //计算卷积窗口
            switch (lineshapeType)
            {
                case "Gaussian":
                    f = x.Map(xi => Math.Exp(-Math.Pow(((2 * Math.Sqrt(2 * Math.Log(2))) * (xi / H_G)), 2)));
                    break;
                case "Lorentzian":
                    f = x.Map(xi => H_L / (4 * Math.Pow(xi, 2) + Math.Pow(H_L, 2)));
                    break;
                case "Voigt":
                    f = x.Map(xi => (k * H_L) / (4 * Math.Pow(xi, 2) + Math.Pow(H_L, 2)) * (2 / Math.PI) +
                                (1 - k) * (Math.Sqrt(2 * Math.Log(2)) / Math.Sqrt(Math.PI) * H_G) *
                                Math.Exp(-Math.Pow(((2 * Math.Sqrt(2 * Math.Log(2))) * (xi / H_G)), 2)));
                    break;
                default:
                    throw new ArgumentException("Gaussian、Lorentzian、Voigt");
            }
            double C_sum = f.Sum();
            Vector<double> C = f - C_sum / windowWidth;
            Vector<double> yTransformed = y.Convolve(C);
            double yTransformedMean = yTransformed.Average();
            double yTransformedStd = yTransformed.StandardDeviation();
            Vector<double> T = yTransformed / yTransformedStd;
            int[] peakPositions = FindAboveIndex(T, thresholdFactor);
            return (peakPositions, yTransformed, T);
        }
        
        public static class VectorExtensions
    {
        /// <summary>
        /// 卷积操作
        /// </summary>
        /// <param name="input">输入信号</param>
        /// <param name="kernel">卷积核</param>
        /// <returns></returns>
        public static Vector<double> Convolve(this Vector<double> input, Vector<double> kernel)
        {
            //初始变量
            int n = input.Count;
            int k = kernel.Count;
            //卷积结果向量的元素数量
            int outputSize = n + k - 1;
            var output = Vector<double>.Build.Dense(outputSize);
            for (int i = 0; i < outputSize; i++)
            {
                //累加和
                double sum = 0;
                //窗口内的乘积累加到sum中
                for (int j = 0; j < k; j++)
                {
                    if (i - j >= 0 && i - j < n)
                    {
                        sum += input[i - j] * kernel[j];
                    }
                }
                output[i] = sum;
            }
            //提取子向量，去掉卷积后向量两端多余的部分,确保输出和输入向量有相同长度
            output = output.SubVector((k - 1) / 2, n);
            return output;
        }
    }
    
    private static int[] FindAboveIndex(Vector<double> T, double thresholdFactor)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < T.Count; i++)
            {
                if (T[i] > thresholdFactor)
                {
                    indices.Add(i);
                }
            }
            return indices.ToArray();
        }
