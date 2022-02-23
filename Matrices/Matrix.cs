using System;
using System.Linq;

namespace RenzLibraries
{
    public class Matrix {
        double[][] matrix;

        public Matrix() {}
        /*
         [[1,0],
          [0,1]]
        */
        
        public Matrix Cholesky() {
            return _Cholesky();
        }

        public string ToString() {
            return matrix.ToString();
        }

        public Matrix Cholesky(bool doOutputUpperTriangle) {
            if (doOutputUpperTriangle) {
                return _Cholesky().Transpose();
            } else {
                return _Cholesky();
            }
        }

        private Matrix _Cholesky() {//Source: https://rosettacode.org/wiki/Cholesky_decomposition
            if (!IsSymmetric()) { throw new Exception($"Cannot get Cholesky as Matrix is not symmetric"); }
            int n = GetRow(0).Length;
            Matrix factor = new Matrix(n, n);

            double sum, diagonal, offDiagonal;
            for (int row = 0; row < n; row++) {
                for (int col = 0; col <= row; col++) {
                    sum = 0;
                    if (row==col) { //Calculate Diagonal
                        for (int j = 0; j < col; j++) {
                            sum += Math.Pow(factor.Get(col, j), 2);
                        }
                        diagonal = Get(col, col) - sum;
                        if (diagonal < 0) {
                            throw new Exception($"Cannot get Cholesky of Matrix as it's Negative Definite or Negative Semidefinite (attempting to square root {diagonal} )");
                        }
                        factor.Set(col, col, Math.Sqrt(diagonal));

                    } else {    //Calculate Off-Diagonal
                        for (int j = 0; j < col; j++) {
                            sum += factor.Get(row, j) * factor.Get(col,j);
                        }
                        offDiagonal = 1d / factor.Get(col, col) * (Get(row, col) - sum);
                        if (double.IsNaN(offDiagonal)) {    offDiagonal = 0;    }
                        factor.Set(row, col, offDiagonal);
                    }
                }
            }
            return factor;
        }

        public bool IsSymmetric() {
            //Matrix A is symmetric when A = Transpose(A)
            Matrix transpose = Transpose();
            if (GetRow(0).Length != transpose.GetColumn(0).Length) {
                return false;
            }

            for (int i = 0; i < GetRow(0).Length; i++) {
                if (    !GetRow(i).SequenceEqual(transpose.GetRow(i))    ) {
                    return false;
                }
            }
            return true;
        }

        public Matrix(int rowSize, int colSize) {
            double[][] emptyMatrix = new double[rowSize][];
            for (int i = 0; i < rowSize; i++) {
                emptyMatrix[i] = new double[colSize];
            }
            matrix = emptyMatrix;
        }
        public void Set(int row, int col, double val) {
            matrix[row][col] = val;
        }

        public void SetRow(int row, double[] vals) {
            for (int i = 0; i < vals.Length; i++) {
                matrix[row][i] = vals[i];
            }
        }

        public double Get(int row, int col) {
            return matrix[row][col];
        }

        public double[] GetColumn(int col) {
            double[] column = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++) {
                column[i] = matrix[i][col];
            }
            return column;
        }
        public double[] GetRow(int row) {
            return matrix[row];
        }
        public double[][] GetDouble() {
            return matrix;
        }

        public Matrix Multiply(Matrix m) {
            int rowCheck = m.GetColumn(0).Length;
            int colCheck = matrix[0].Length;
            if (rowCheck != colCheck) {
                throw new Exception("Cannot multiply matrices as multiplicand's column length is not the same as multiplier's row length");
            };

            double dotProduct = 0;
            Matrix product = new Matrix(matrix.Length, m.GetRow(0).Length);
            for (int i = 0; i < matrix.Length; i++) {
                double[] row = matrix[i];

                for (int j = 0; j < m.GetRow(0).Length; j++) {
                    for (int k = 0; k < row.Length; k++) {
                        double a = matrix[i][k];
                        double b = m.Get(k, j);
                        dotProduct += a * b;
                    }
                    product.Set(i, j, dotProduct);
                    dotProduct = 0;
                }
            }
            return product;
        }

        public Matrix Multiply(double s) {
            Matrix product = new Matrix(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; i++) {
                for (int j = 0; j < matrix[0].Length; j++) {
                    product.Set(i, j, matrix[i][j] * s);
                }
            }
            return product;
        }

        public Matrix Transpose() {
            Matrix Transpose = new Matrix(matrix[0].Length, matrix.Length);
            for (int i = 0; i < matrix.Length; i++) {
                for (int j = 0; j < matrix[0].Length; j++) {
                    Transpose.Set(j, i, matrix[i][j]);
                }
            }
            return Transpose;
        }

        public Matrix Add(Matrix m) {
            Matrix A = new Matrix(matrix.Length, matrix[0].Length);
            int rowLen = m.GetRow(0).Length;
            int colLen = m.GetColumn(0).Length;
            if (colLen == matrix.Length &&
                rowLen == matrix[0].Length) {
                for (int i = 0; i < matrix.Length; i++) {
                    for (int j = 0; j < matrix[0].Length; j++) {
                        A.Set(i, j, m.Get(i, j) + matrix[i][j]);
                    }
                }
            } else {
                throw new Exception("Cannot add Matrices as they're not the same dimensions");
            }
            return A;
        }

        public Matrix Subtract(Matrix m) {
            Matrix A = new Matrix(matrix.Length, matrix[0].Length);
            int rowLen = m.GetRow(0).Length;
            int colLen = m.GetColumn(0).Length;
            if (colLen == matrix.Length &&
                rowLen == matrix[0].Length) {
                for (int i = 0; i < matrix.Length; i++) {
                    for (int j = 0; j < matrix[0].Length; j++) {
                        A.Set(i, j, matrix[i][j] - m.Get(i, j));
                    }
                }
            } else {
                throw new Exception("Cannot subtract Matrices as they're not the same dimensions");
            }
            return A;
        }

        public Matrix Invert_2x2() {
            if (matrix.Length != 2 && matrix[0].Length != 2) {
                if (matrix.Length != 1 && matrix[0].Length != 1) {
                    throw new Exception("Matrix must be 2x2 or 1x1");
                }
            }

            if (Determinant() == 0) {
                throw new Exception("Cannot invert Matrix as it's determinant is zero");
            }

            //2x2 Inverse
            if (matrix.Length == 2 && matrix[0].Length == 2) {
                Matrix inverse = new Matrix(2, 2);
                double a = matrix[0][0];
                double b = matrix[0][1];
                double c = matrix[1][0];
                double d = matrix[1][1];

                double denominator = (a * d) - (b * c);
                inverse.Set(0, 0, d / denominator);
                inverse.Set(0, 1, -b / denominator);
                inverse.Set(1, 0, -c / denominator);
                inverse.Set(1, 1, a / denominator);

                return inverse;
                //1x1 Inverse
            } else {
                Matrix inverse = new Matrix(1, 1);
                inverse.Set(0, 0, 1 / matrix[0][0]);
                return inverse;
            }


        }

        public double Determinant() {
            if (matrix.Length != matrix[0].Length) { throw new Exception("Cannot get determinant as Matrix is not square"); };
            double result = 0;
            if (matrix.Length == 1) {
                result = matrix[0][0];
                return result;
            }

            if (matrix.Length == 2) {
                result = matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
                return result;
            }

            for (int i = 0; i < matrix[0].Length; i++) {
                double[][] temp = new double[matrix.Length - 1][];
                for (int j = 0; j < temp.Length; j++) {
                    temp[j] = new double[temp.Length];
                }

                for (int k = 1; k < matrix.Length; k++) {
                    Array.Copy(matrix[k], 0, temp[k - 1], 0, i);
                    Array.Copy(matrix[k], i + 1, temp[k - 1], i, matrix[0].Length - i - 1);
                }

                Matrix tempMatrix = new Matrix(temp.Length, temp.Length);
                for (int l = 0; l < temp.Length; l++) {
                    tempMatrix.SetRow(l, temp[l]);
                }
                result += matrix[0][i] * Math.Pow(-1, i) * tempMatrix.Determinant();

            }
            return result;
        }
    }
}
