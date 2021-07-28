using NUnit.Framework;

namespace RenzLibraries.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void MatrixTranspose_Assigned() {
            Matrix A = new Matrix(4, 3);
            A.SetRow(0, new double[] { 1d, 0d, -1d });
            A.SetRow(1, new double[] { 2d, 7d, -5d });
            A.SetRow(2, new double[] { 4d, -3d, 2d });
            A.SetRow(3, new double[] { -1d, 3d, 0d });
            //[[1,0,-1],[2,7,-5],[4,-3,2],[-1,3,0]]

            //[[1,2,4,-1],[0,7,-3,3],[-1,-5,2,0]]
            Matrix B = A.Transpose();
            Assert.AreEqual(B.GetColumn(0).Length, 3);
            Assert.AreEqual(B.GetRow(0).Length, 4);
            Assert.AreEqual(B.Get(0, 1), 2);
            Assert.AreEqual(B.Get(1, 2), -3);
            Assert.AreEqual(B.Get(1, 3), 3);
        }

        [Test]
        public void MatrixSet_Assigned() {
            double a = 2;
            double[] value = new double[] { 1, a };
            Matrix m = new Matrix(3, 2);
            m.SetRow(1, value);
            Assert.AreEqual(m.Get(1, 1), a);
            //[[0,0],[1,2],[0,0]]

            double b = 3;
            m.Set(1, 1, b);
            Assert.AreEqual(m.Get(1, 1), b);
        }

        [Test]
        public void MatrixAdd_Returns() {
            Matrix A = new Matrix(2, 3);
            Matrix B = new Matrix(2, 3);

            A.SetRow(0, new double[] { 2, 3, 5 });
            A.SetRow(1, new double[] { 7, 11, 13 });
            B.SetRow(0, new double[] { 17, 19, 23 });
            B.SetRow(1, new double[] { 29, 31, 33 });

            Matrix C = A.Add(B);
            Assert.AreEqual(C.Get(0, 0), 19);
            Assert.AreEqual(C.Get(1, 1), 42);
            Assert.AreEqual(C.Get(1, 2), 46);
        }

        [Test]
        public void MatrixSubtract_Returns() {
            Matrix A = new Matrix(2, 3);
            Matrix B = new Matrix(2, 3);

            A.SetRow(0, new double[] { 2, 3, 5 });
            A.SetRow(1, new double[] { 7, 11, 13 });
            B.SetRow(0, new double[] { 17, 19, 23 });
            B.SetRow(1, new double[] { 29, 31, 33 });

            Matrix C = A.Subtract(B);
            Assert.AreEqual(C.Get(0, 0), -15);
            Assert.AreEqual(C.Get(1, 0), -22);
            Assert.AreEqual(C.Get(1, 2), -20);
        }

        [Test]
        public void MatrixGetColumn_Returns() {
            Matrix A = new Matrix(2, 3);
            double[] A1 = new double[] { 0, 3, 5 };
            double[] A2 = new double[] { 5, 5, 2 };
            A.SetRow(0, A1);
            A.SetRow(1, A2);

            /*[[0,3,5], 
               [5,5,2]]
            */
            double[] column = A.GetColumn(1);
            Assert.AreEqual(column[0], 3);
            Assert.AreEqual(column[1], 5);
            Assert.AreEqual(A.Get(1, 2), 2);
        }

        [Test]
        public void MatrixGetRow_Returns() {
            Matrix A = new Matrix(2, 3);
            double[] A1 = new double[] { 0, 3, 5 };
            double[] A2 = new double[] { 5, 5, 2 };
            A.SetRow(0, A1);
            A.SetRow(1, A2);

            /*[[0,3,5], 
               [5,5,2]]
            */
            double[] row = A.GetRow(1);
            Assert.AreEqual(row[0], 5);
            Assert.AreEqual(row[1], 5);
            Assert.AreEqual(row[2], 2);
        }

        [Test]
        public void MatrixMultiply_Scalar() {
            Matrix A = new Matrix(2, 3);
            A.SetRow(0, new double[] { 1, 2, 3 });
            A.SetRow(1, new double[] { 4, 5, 6 });

            Matrix B = A.Multiply(2d);
            Assert.AreEqual(B.Get(0, 0), 2);
            Assert.AreEqual(B.Get(1, 0), 8);
            Assert.AreEqual(B.Get(1, 2), 12);
        }

        [Test]
        public void MatrixMultiply_Product() {
            Matrix A = new Matrix(2, 3);
            double[] A1 = new double[] { 0, 3, 5 };
            double[] A2 = new double[] { 5, 5, 2 };
            A.SetRow(0, A1);
            A.SetRow(1, A2);
            //[[0,3,5], 
            // [5,5,2]]

            Matrix B = new Matrix(3, 2);
            double[] B1 = new double[] { 3, 4 };
            double[] B2 = new double[] { 3, -2 };
            double[] B3 = new double[] { 4, -2 };
            B.SetRow(0, B1);
            B.SetRow(1, B2);
            B.SetRow(2, B3);
            //[[3,4],
            // [3,-2],
            // [4,-2]]

            //[[29,-16], [38,6]]
            Matrix product = A.Multiply(B);
            Assert.AreEqual(product.Get(0, 0), 29d);
            Assert.AreEqual(product.Get(0, 1), -16d);
            Assert.AreEqual(product.Get(1, 0), 38d);
            Assert.AreEqual(product.Get(1, 1), 6d);

            Assert.AreEqual(product.GetColumn(0).Length, 2);
            Assert.AreEqual(product.GetRow(0).Length, 2);

            product = B.Multiply(A);
            Assert.AreEqual(product.Get(0, 0), 20d);
            Assert.AreEqual(product.Get(0, 1), 29d);
            Assert.AreEqual(product.Get(0, 2), 23d);
            Assert.AreEqual(product.Get(1, 0), -10d);
            Assert.AreEqual(product.Get(1, 1), -1d);
            Assert.AreEqual(product.Get(1, 2), 11d);
            Assert.AreEqual(product.Get(2, 0), -10d);
            Assert.AreEqual(product.Get(2, 1), 2d);
            Assert.AreEqual(product.Get(2, 2), 16d);
        }

        [Test]
        public void MatrixDeterminant_Returns() {
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
        public void MatrixInverse2x2_Returns() {
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