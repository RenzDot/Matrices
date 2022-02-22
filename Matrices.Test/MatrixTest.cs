using NUnit.Framework;

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
        public void Matrix_IsSymmetric_SymmetricInputIsTrue() {
            Matrix A = new Matrix(3, 3);
            A.SetRow(0, new double[] { 1, 7, 3});
            A.SetRow(1, new double[] { 7, 4, 5});
            A.SetRow(2, new double[] { 3, 5, 0});

            //Assert.IsFalse(MatrixD.IsSymmetric());
            Assert.IsTrue(A.IsSymmetric());
        }

        [Test]
        public void Matrix_Cholesky_AssignsLowerTriangle() {
            Matrix A = new Matrix(3, 3);
            A.SetRow(0, new double[] {4, 2, -2});
            A.SetRow(1, new double[] {2, 1, -1});
            A.SetRow(2, new double[] {-2, -1, 10});
            /*
            [ 4  -2  -6]        [ 2   0   0]
            [-2  10   9]        [-1   3   0]
            [-6   9  14]    ->  [-3   2   1]
            */

            Matrix cholesky = A.Cholesky();
            Assert.IsTrue(false);
        }

        [Test]
        public void Matrix_Cholesky_SingularInputCreatesSinglarOutput() {
            Assert.IsTrue(false);
            /*
            [1 1]
            [1 1]   ->  Singular Output with determinant = 0
            */
        }

        [Test]
        public void Matrix_Cholesky_NegativeSemidefiniteInputException() {
            Assert.IsTrue(false);
            /*
            [ 1  2 -1]
            [ 2  5  1]
            [-1  1  9]  -> Error on final step, calling for a square root of -1
            */
        }

        [Test]
        public void Matrix_Cholesky_NonSymmetricInputException() {
            Assert.IsTrue(false);
            /*
            [2.459 1.153]
            [0.    2.496]   ->  Error

            [ 0.057  0.031  0.    -0.   ]
            [ 0.     0.03  -0.     0.   ]
            [ 0.     0.     0.057  0.031]
            [ 0.     0.     0.     0.03 ]   -> Error
            */
        }

        public void Matrix_Cholesky_AssignsValueOnIndeterminant() {
            /*            
            [ 4  -2   2]        [ 2  0  0]
            [-2   1  -1]        [-1  0  0]
            [ 2  -1   5]    ->  [ 1  0  2] 
            Source: https://www.value-at-risk.net/cholesky-factorization/ 
            Here, x is indeterminant and is set to zero
            */
        }

        [Test]
        public void Matrix_Cholesky_AssignsValue() {
            Matrix A = new Matrix(2, 2);
            A.SetRow(0, new double[] { 6.048, 2.835 });
            A.SetRow(1, new double[] { 2.835, 7.56 });

            Matrix cholesky = A.Cholesky();

            /*
            For Kalman Filter, Lambda will make the input symmetrical for Cholesky
            Source: Test Values taken from IPython Kalman Filter
            [6.048 2.835]    [2.459 1.153]
            [2.835 7.56 ] -> [0.    2.496] 

            [1.08 0.54]     [1.039 0.52 ]
            [0.54 1.08] ->  [0.    0.9  ]

            [16.38   2.633 -0.71 ]      [ 4.047  0.65  -0.175]
            [ 2.633  0.435  0.01 ]      [ 0.     0.111  1.118]
            [-0.71   0.01   6.848]  ->  [ 0.     0.     2.359]

            [16.455  2.631 -1.027]      [ 4.056  0.649 -0.253]
            [ 2.631  0.436  0.019]      [ 0.     0.122  1.507]
            [-1.027  0.019  7.396]  ->  [ 0.     0.     2.25 ]

            [0.05 0.   0.   0.  ]       [0.224 0.    0.    0.   ]
            [0.   0.05 0.   0.  ]       [0.    0.224 0.    0.   ]
            [0.   0.   0.05 0.  ]       [0.    0.    0.224 0.   ]
            [0.   0.   0.   0.05]   ->  [0.    0.    0.    0.224]

            [ 0.003  0.002  0.    -0.   ]       [ 0.057  0.031  0.    -0.   ]
            [ 0.002  0.002 -0.     0.   ]       [ 0.     0.03  -0.     0.   ]
            [ 0.    -0.     0.003  0.002]       [ 0.     0.     0.057  0.031]
            [-0.     0.     0.002  0.002]   ->  [ 0.     0.     0.     0.03 ]
            */
            Assert.IsTrue(false);
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
        public void Matrix_SetRow_AssignsValue() {
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
        public void Matrix_GetColumn_Returns() {
            double[] secondColumn = MatrixA.GetColumn(1);
            Assert.AreEqual(secondColumn[0], MatrixA.Get(0,1));
            Assert.AreEqual(secondColumn[1], MatrixA.Get(1,1));
        }

        [Test]
        public void Matrix_GetRow_Returns() {
            double[] firstRow = MatrixA.GetRow(0);
            Assert.AreEqual(firstRow[0], MatrixA.Get(0,0));
            Assert.AreEqual(firstRow[1], MatrixA.Get(0,1));
            Assert.AreEqual(firstRow[2], MatrixA.Get(0,2));
        }

        [Test]
        public void Matrix_Multiply_Returns() {
            double scalar = 2d;
            Matrix product = MatrixA.Multiply(scalar);
            Assert.AreEqual(product.Get(0,0), MatrixA.Get(0,0) * scalar);
            Assert.AreEqual(product.Get(1,0), MatrixA.Get(1,0) * scalar);
            Assert.AreEqual(product.Get(2,2), MatrixA.Get(2,2) * scalar);
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
        public void Matrix_Multiply_RejectWrongInputSize() {
            Matrix A = new Matrix(1, 2);
            Matrix B = new Matrix(1, 2);

            Assert.Throws<System.Exception>( ()=> { A.Multiply(B); }  );
        }

        [Test]
        public void Matrix_Multiply_CorrectProductValues() {
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
        public void Matrix_Determinant_Returns() {
            Matrix A = new Matrix(2, 2);
            A.SetRow(0, new double[] { 0, 3 });
            A.SetRow(1, new double[] { 4, 7 });

            Matrix B = new Matrix(3, 3);
            B.SetRow(0, new double[] { 0, 2, 3 });
            B.SetRow(1, new double[] { 5, 7, 13 });
            B.SetRow(2, new double[] { 17, 19, 23 });

            Assert.AreEqual(Matrix.Determinant(A.GetDouble()), -12);
            Assert.AreEqual(Matrix.Determinant(B.GetDouble()), 140);
        }

        [Test]
        public void Matrix_Inverse2x2_Returns() {
            Matrix A = new Matrix(2, 2);
            A.SetRow(0, new double[] { 4, 7 });
            A.SetRow(1, new double[] { 2, 6 });

            Matrix B = Matrix.Invert_2x2(A.GetDouble());
            Assert.AreEqual(B.Get(0, 0), 0.6);
            Assert.AreEqual(B.Get(0, 1), -0.7);
            Assert.AreEqual(B.Get(1, 0), -0.2);
            Assert.AreEqual(B.Get(1, 1), 0.4);
        }
    }
}