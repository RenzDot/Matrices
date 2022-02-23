using NUnit.Framework;
using System;
using System.Linq;

namespace RenzLibraries.Test
{
    public class Tests
    {
        Matrix MatrixA, MatrixB, MatrixC, MatrixD;
        [SetUp]
        public void Setup() {
            MatrixA = new Matrix(2, 3);
            MatrixA.SetRow(0, new double[] { 2, 3, 5 });
            MatrixA.SetRow(1, new double[] { 7, 11, 13 });

            MatrixB = new Matrix(2, 3);
            MatrixB.SetRow(0, new double[] { 17, 19, 23 });
            MatrixB.SetRow(1, new double[] { 29, 31, 33 });

            MatrixC = new Matrix(4, 3);
            MatrixC.SetRow(0, new double[] { 1d, 0d, -1d });
            MatrixC.SetRow(1, new double[] { 2d, 7d, -5d });
            MatrixC.SetRow(2, new double[] { 4d, -3d, 2d });
            MatrixC.SetRow(3, new double[] { -1d, 3d, 0d });

            MatrixD = new Matrix(3, 4);
            MatrixD.SetRow(0, new double[] { 1d, 2d, 4d,-1d });
            MatrixD.SetRow(1, new double[] { 0d, 7d,-3d, 3d });
            MatrixD.SetRow(2, new double[] {-1d,-5d, 2d, 0d });
        }

        [Test]
        public void Matrix_IsSymmetric_TrueWhenSymmetric() {
            Matrix A = new Matrix(3, 3);
            A.SetRow(0, new double[] { 1, 7, 3});
            A.SetRow(1, new double[] { 7, 4, 5});
            A.SetRow(2, new double[] { 3, 5, 0});

            Assert.IsFalse(MatrixD.IsSymmetric());
            Assert.IsTrue(A.IsSymmetric());
        }

        [Test]
        public void Matrix_Cholesky_OutputsUpperTriangleMatrix() {
            Matrix A = new Matrix(3, 3);
            A.SetRow(0, new double[] {4, -2, -6});
            A.SetRow(1, new double[] {-2, 10, 9});
            A.SetRow(2, new double[] {-6, 9, 14});

            Matrix choleskyExpected = new Matrix(3, 3);
            choleskyExpected.SetRow(0, new double[] { 2, -1, -3 });
            choleskyExpected.SetRow(1, new double[] { 0, 3, 2 });
            choleskyExpected.SetRow(2, new double[] { 0, 0, 1 });

            Matrix choleskyActual = A.Cholesky(true);
            Assert.AreEqual(choleskyExpected.GetRow(0), choleskyActual.GetRow(0));
            Assert.AreEqual(choleskyExpected.GetRow(1), choleskyActual.GetRow(1));
            Assert.AreEqual(choleskyExpected.GetRow(2), choleskyActual.GetRow(2));
        }

        [Test]
        public void Matrix_Cholesky_SingularInputOutputsSinglar() {
            Matrix A = new Matrix(2,2);
            A.SetRow(0, new double[] { 1, 1 });
            A.SetRow(1, new double[] { 1, 1 });

            Matrix cholesky = A.Cholesky();
            Assert.IsTrue(cholesky.Determinant() == 0);//Singular Outputs have determinant == 0
        }

        [Test]
        public void Matrix_Cholesky_InputtingNegativeSemidefiniteMatricesGivesError() {
            Matrix A = new Matrix(3, 3);
            A.SetRow(0, new double[] { 1, 2,-1});
            A.SetRow(1, new double[] { 2, 5, 1});
            A.SetRow(2, new double[] {-1, 1, 9});

            //Should error on last matrix element attempting to square root -1
            var exception = Assert.Throws<Exception>(() => A.Cholesky());
            Assert.IsTrue(exception.Message.StartsWith("Cannot get Cholesky of Matrix as it's Negative Definite or Negative Semidefinite"));
        }

        [Test]
        public void Matrix_Cholesky_InputtingNonSymmetricInputGivesError() {
            var exception = Assert.Throws<Exception>(() => MatrixA.Cholesky());
            Assert.IsTrue(exception.Message.Equals("Cannot get Cholesky as Matrix is not symmetric"));
        }

        [Test]
        public void Matrix_Cholesky_NoErrorOnAssigningIndeterminantElements() {
            //Source: https://www.glynholton.com/solutions/exercise-solution-2-10/
            Matrix A = new Matrix(3, 3);
            A.SetRow(0, new double[] { 4, -2, 2 });
            A.SetRow(1, new double[] {-2, 1, -1 });
            A.SetRow(2, new double[] { 2, -1, 5 });
            Matrix choleskyActual = A.Cholesky();

            Assert.AreEqual(new double[] { 2, 0, 0 }, choleskyActual.GetRow(0));
            Assert.AreEqual(new double[] {-1, 0, 0 }, choleskyActual.GetRow(1));
            Assert.AreEqual(new double[] { 1, 0, 2 }, choleskyActual.GetRow(2));
        }

        [Test]
        public void Matrix_Cholesky_AssignsCorrect3x3Matrix() {
            Matrix C = new Matrix(3, 3);
            C.SetRow(0, new double[] { 16.38, 2.633, -0.71 });
            C.SetRow(1, new double[] { 2.633, 0.435, 0.01 });
            C.SetRow(2, new double[] { -0.71, 0.01, 6.848 });

            Matrix cholesky = C.Cholesky();
            Assert.AreEqual(new double[] { 4.047, 0, 0 }, cholesky.GetRow(0).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0.651, 0.108, 0 }, cholesky.GetRow(1).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { -0.175, 1.145, 2.347 }, cholesky.GetRow(2).Select(d => Math.Round(d, 3)).ToArray());
        }

        [Test]
        public void Matrix_Cholesky_AssignsCorrectNxNMatrix() {
            Matrix A = new Matrix(2, 2);
            A.SetRow(0, new double[] { 6.048, 2.835});
            A.SetRow(1, new double[] { 2.835, 7.56 });

            Matrix B = new Matrix(2, 2);
            B.SetRow(0, new double[] { 1.08, 0.54 });
            B.SetRow(1, new double[] { 0.54, 1.08 });

            Matrix D = new Matrix(3, 3);
            D.SetRow(0, new double[] { 16.455, 2.631, -1.027});
            D.SetRow(1, new double[] { 2.631, 0.435,   0.01 });
            D.SetRow(2, new double[] { -1.027, 0.01,  7.396 });

            Matrix E = new Matrix(4, 4);
            E.SetRow(0, new double[] { 0.05, 0, 0, 0 });
            E.SetRow(1, new double[] { 0, 0.05, 0, 0 });
            E.SetRow(2, new double[] { 0, 0, 0.05, 0 });
            E.SetRow(3, new double[] { 0, 0, 0, 0.05 });

            Matrix F = new Matrix(4, 4);
            F.SetRow(0, new double[] { 0.003, 0.002, 0, 0 });
            F.SetRow(1, new double[] { 0.002, 0.002, 0, 0 });
            F.SetRow(2, new double[] { 0, 0, 0.003, 0.002 });
            F.SetRow(3, new double[] { 0, 0, 0.002, 0.002 });

            Matrix cholesky = A.Cholesky();
            Assert.AreEqual(new double[] { 2.459, 0d }, cholesky.GetRow(0).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 1.153, 2.496 }, cholesky.GetRow(1).Select(d => Math.Round(d, 3)).ToArray());

            cholesky = B.Cholesky();
            Assert.AreEqual(new double[] { 1.039, 0 }, cholesky.GetRow(0).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0.52, 0.9 }, cholesky.GetRow(1).Select(d => Math.Round(d, 3)).ToArray());

            cholesky = D.Cholesky();
            Assert.AreEqual(new double[] { 4.056, 0, 0 }, cholesky.GetRow(0).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0.649, 0.12, 0 }, cholesky.GetRow(1).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { -0.253, 1.455, 2.283 }, cholesky.GetRow(2).Select(d => Math.Round(d, 3)).ToArray());

            cholesky = E.Cholesky();
            Assert.AreEqual(new double[] { 0.224, 0, 0, 0 }, cholesky.GetRow(0).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0, 0.224, 0, 0 }, cholesky.GetRow(1).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0, 0, 0.224, 0 }, cholesky.GetRow(2).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0, 0, 0, 0.224 }, cholesky.GetRow(3).Select(d => Math.Round(d, 3)).ToArray());

            cholesky = F.Cholesky();
            Assert.AreEqual(new double[] { 0.055, 0, 0, 0 }, cholesky.GetRow(0).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0.037, 0.026, 0, 0 }, cholesky.GetRow(1).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0, 0, 0.055, 0 }, cholesky.GetRow(2).Select(d => Math.Round(d, 3)).ToArray());
            Assert.AreEqual(new double[] { 0, 0, 0.037, 0.026 }, cholesky.GetRow(3).Select(d => Math.Round(d, 3)).ToArray());
        }

        [Test]
        public void Matrix_Transpose_CorrectTransposeSize() {
            Matrix Transpose = MatrixC.Transpose();
            Assert.AreEqual(Transpose.GetColumn(0).Length, MatrixD.GetColumn(0).Length);
            Assert.AreEqual(Transpose.GetRow(0).Length, MatrixD.GetRow(0).Length);
        }

        [Test]
        public void Matrix_Transpose_CorrectTransposeValues() {
            Matrix Transpose = MatrixC.Transpose();
            for (int i = 0; i < Transpose.GetColumn(0).Length; i++) {
                Assert.AreEqual(Transpose.GetRow(i), MatrixD.GetRow(i));
            }
        }

        [Test]
        public void Matrix_SetRow_AssignsRow() {
            double a = 2;
            double[] value = new double[] { 1, a };

            Matrix m = new Matrix(3, 2);
            m.SetRow(1, value);

            Assert.AreEqual(m.Get(1, 1), a);
            //[[0,0],[1,2],[0,0]]
        }

        [Test]
        public void Matrix_Set_AssignsValue() {
            double b = 3;

            Matrix m = new Matrix(3, 2);
            m.Set(2, 1, b);
            Assert.AreEqual(m.Get(2, 1), b);
        }

        [Test]
        public void Matrix_Add_CorrectSum() {
            Matrix A = new Matrix(2, 3);
            Matrix B = new Matrix(2, 3);

            A.SetRow(0, new double[] { 2, 3, 5 });
            A.SetRow(1, new double[] { 7, 11, 13 });

            B.SetRow(0, new double[] { 17, 19, 23 });
            B.SetRow(1, new double[] { 29, 31, 33 });

            Matrix sum = A.Add(B);
            Assert.AreEqual(sum.Get(0, 0), 19);
            Assert.AreEqual(sum.Get(1, 1), 42);
            Assert.AreEqual(sum.Get(1, 2), 46);
        }

        [Test]
        public void Matrix_Subtract_CorrectSum() {
            Matrix A = new Matrix(2, 3);
            Matrix B = new Matrix(2, 3);

            A.SetRow(0, new double[] { 2, 3, 5 });
            A.SetRow(1, new double[] { 7, 11, 13 });

            B.SetRow(0, new double[] { 17, 19, 23 });
            B.SetRow(1, new double[] { 29, 31, 33 });

            Matrix sum = A.Subtract(B);
            Assert.AreEqual(sum.Get(0, 0), -15);
            Assert.AreEqual(sum.Get(1, 0), -22);
            Assert.AreEqual(sum.Get(1, 2), -20);
        }

        [Test]
        public void Matrix_GetColumn_ReturnsColumn() {
            double[] secondColumn = MatrixA.GetColumn(1);
            Assert.AreEqual(secondColumn[0], MatrixA.Get(0,1));
            Assert.AreEqual(secondColumn[1], MatrixA.Get(1,1));
        }

        [Test]
        public void Matrix_GetRow_ReturnsRow() {
            double[] firstRow = MatrixA.GetRow(0);
            Assert.AreEqual(firstRow[0], MatrixA.Get(0,0));
            Assert.AreEqual(firstRow[1], MatrixA.Get(0,1));
            Assert.AreEqual(firstRow[2], MatrixA.Get(0,2));
        }

        [Test]
        public void Matrix_Multiply_CorrectScalarProduct() {
            double scalar = 2d;
            Matrix product = MatrixA.Multiply(scalar);
            Assert.AreEqual(product.Get(0,0), MatrixA.Get(0,0) * scalar);
            Assert.AreEqual(product.Get(1,0), MatrixA.Get(1,0) * scalar);
            Assert.AreEqual(product.Get(1,2), MatrixA.Get(1,2) * scalar);
        }

        [Test]
        public void Matrix_Multiply_CorrectProductSize() {
            Matrix product1 = MatrixA.Multiply(MatrixD);
            Matrix product2 = MatrixC.Multiply(MatrixD);

            Assert.AreEqual(product1.GetColumn(0).Length, MatrixA.GetColumn(0).Length);
            Assert.AreEqual(product1.GetRow(0).Length, MatrixD.GetRow(0).Length);

            Assert.AreEqual(product2.GetColumn(0).Length, MatrixC.GetColumn(0).Length);
            Assert.AreEqual(product2.GetRow(0).Length, MatrixD.GetRow(0).Length);
        }

        [Test]
        public void Matrix_Multiply_IncorrectMatrixSizeGivesError() {
            Matrix A = new Matrix(1, 2);
            Matrix B = new Matrix(1, 2);

            Assert.Throws<System.Exception>( ()=> { A.Multiply(B); }  );
        }

        [Test]
        public void Matrix_Multiply_CorrectMatrixProduct() {
            Matrix product = MatrixA.Multiply(MatrixD);

            double[] row0 = MatrixA.GetRow(0);
            double[] row1 = MatrixA.GetRow(1);
            double[] col0 = MatrixD.GetColumn(0);
            double[] col3 = MatrixD.GetColumn(3);

            var dot0 = (row0[0] * col0[0]) + (row0[1] * col0[1]) + (row0[2] * col0[2]);
            var dot3 = (row0[0] * col3[0]) + (row0[1] * col3[1]) + (row0[2] * col3[2]);
            var dot7 = (row1[0] * col3[0]) + (row1[1] * col3[1]) + (row1[2] * col3[2]);

            Assert.AreEqual(product.Get(0, 0), dot0);
            Assert.AreEqual(product.Get(0, 3), dot3);
            Assert.AreEqual(product.Get(1, 3), dot7);
        }

        [Test]
        public void Matrix_Determinant_CorrectDeterminant() {
            Matrix A = new Matrix(2, 2);
            A.SetRow(0, new double[] { 0, 3 });
            A.SetRow(1, new double[] { 4, 7 });

            Matrix B = new Matrix(3, 3);
            B.SetRow(0, new double[] { 0, 2, 3 });
            B.SetRow(1, new double[] { 5, 7, 13 });
            B.SetRow(2, new double[] { 17, 19, 23 });

            Assert.AreEqual(A.Determinant(), -12);
            Assert.AreEqual(B.Determinant(), 140);
        }

        [Test]
        public void Matrix_Inverse2x2_CorrectInverse() {
            Matrix A = new Matrix(2, 2);
            A.SetRow(0, new double[] { 4, 7 });
            A.SetRow(1, new double[] { 2, 6 });

            Matrix B = A.Invert_2x2();
            Assert.AreEqual(B.Get(0, 0), 0.6);
            Assert.AreEqual(B.Get(0, 1), -0.7);
            Assert.AreEqual(B.Get(1, 0), -0.2);
            Assert.AreEqual(B.Get(1, 1), 0.4);
        }
    }
}