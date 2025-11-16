using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixCalculator
{
    public static float[,] MultiplyMatrices(float[,] matrixA, float[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int rowsB = matrixB.GetLength(0);
        int colsB = matrixB.GetLength(1);

        // 检查矩阵是否可以相乘
        if (colsA != rowsB)
        {
            Debug.LogError("Matrices cannot be multiplied. Number of columns in matrix A must equal number of rows in matrix B.");
            return null;
        }
        float[,] result = new float[rowsA, colsB];
        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < colsA; k++)
                {
                    result[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        }
        return result;
    }
}
