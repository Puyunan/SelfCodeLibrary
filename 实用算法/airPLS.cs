 public static Tuple<Vector<double>, Vector<double>> AirPLS(Vector<double> x, double lambda = 10e4, int order = 2, double wep = 0.5, double p = 0.05, int itermax = 200)
        {
            int n = x.Count;

            int[] wi = new int[n];
            for (int i = 0; i < n; i++)
            {
                if (i < Math.Ceiling(n * wep) || i >= n - Math.Floor(n * wep))
                    wi[i] = 1;
                else
                    wi[i] = 0;
            }
            Matrix<double> D = Matrix<double>.Build.DenseOfMatrix(CalculateFiniteDifferenceMatrix(n, order));
            Matrix<double> DD = lambda * D.Transpose() * D;
            Vector<double> w = Vector<double>.Build.Dense(n, 1);
            Vector<double> z = Vector<double>.Build.Dense(n);

            //迭代
            for (int j = 0; j < itermax; j++)
            {
                //权重向量转换为对角矩阵
                Matrix<double> W = Matrix<double>.Build.DenseOfDiagonalVector(w);
                //计算正则化矩阵(W+DD)的Cholesky分解
                Matrix<double> C = (W + DD).Cholesky().Factor;
                //用Cholesky分解求解线性方程组(W + DD) * z = W * x
                z = C.Transpose().Solve(C.Solve(W * x));
                //原始信号与基线向量差异
                Vector<double> d = x - z;
                //计算负差异值的L1范数
                double dssn = d.PointwiseMultiply(d.Map(v => v < 0 ? 1.0 : 0.0)).L1Norm();
                //负差异的L1范数小于原始信号L1范数的0.001倍
                if (dssn < 0.001 * x.L1Norm())
                    break;
                //更新权重向量
                for (int k = 0; k < n; k++)
                {
                    if (d[k] >= 0)
                        w[k] = 0;
                    else if (wi[k] == 1)
                        w[k] = p;
                    else
                        w[k] = Math.Exp(j * Math.Abs(d[k]) / dssn);
                }
            }

            Vector<double> Xc = x - z;
            return Tuple.Create(Xc, z);
        }
        
        private static Matrix<double> CalculateFiniteDifferenceMatrix(int n, int order)
        {
            Matrix<double> eye = Matrix<double>.Build.DenseIdentity(n, n);
            Matrix<double> diffMatrix = eye;
            for (int i = 0; i < order; i++)
                diffMatrix = Difference(diffMatrix, 1);
            return diffMatrix;
        }
        
        private static Matrix<double> Difference(Matrix<double> matrix, int dimension)
        {
            int rows = matrix.RowCount;
            int cols = matrix.ColumnCount;
            Matrix<double> diffMatrix = Matrix<double>.Build.Dense(rows - dimension, cols);
            for (int i = 0; i < rows - dimension; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    diffMatrix[i, j] = matrix[i + dimension, j] - matrix[i, j];
                }
            }
            return diffMatrix;
        }
        
