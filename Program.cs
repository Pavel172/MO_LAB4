using System;
using System.Collections.Generic;

namespace MO_LAB3
{
    public class Programm
    {
        public static void Main()
        {
            int b = -16, flag = 0;
            List<double> vec_c = new List<double>() { 6, 9, 5, 8, 5, 8, 6, 8 };
            vec_c.Sort(); //сортируем вектор c от наименьшего к наибольшему значению
            List<double> vec_a = new List<double>() { -5, -1, 9, -5, 5, 9, 6, -6 };
            string aspiration = "min";
            string sign = "<=";
            if(sign == "max") //если функция стремится к max, то меняем стремление на min
            {
                flag = 1; //чтобы в будущем можно было снова вернуть стремление функции к max
                for(int i = 0; i < vec_c.Count; ++i)
                {
                    if (vec_c[i] == 0) continue;
                    vec_c[i] = (-1) * vec_c[i];
                }
            }
            List<double> answer = new List<double>(); //отдельное решение
            List<List<double>> answer_list = new List<List<double>>(); //список со всеми допустимыми решениями
            double F_min = 100000, F = 0, sum = 0;
            Console.WriteLine("Метод полного перебора: ");
            List<List<double>> combinations = new List<List<double>>(); //список, в котором будут находиться все возможные комбинации 0 и 1 для переменных
            GenerateCombinations(vec_c.Count, new List<double>(), ref combinations); //ищем все возможные комбинации 0 и 1
            for (int i = 0; i < combinations.Count; ++i) //проверяем каждую комбинацию на допустимость
            {
                sum = 0;
                F = 0;
                for (int j = 0; j < combinations[i].Count; ++j) 
                {
                    F += vec_c[j] * combinations[i][j]; //находим значение функции при данных иксах
                    sum += vec_a[j] * combinations[i][j]; //находим сумму иксов для проверки ограничений
                }
                if (sum > b) continue; //проверяем ограничения
                if (F_min > F) F_min = F; //находим минимальное значение функции среди решений
                answer = new List<double>(combinations[i]);
                answer.Insert(0, F);
                answer_list.Add(answer); //добавляем ответ в список ответов
            }
            for(int i = 0; i < answer_list.Count; ++i) //выводим в консоль все допустимые решения и показываем их допустимость
            {
                if (flag == 1) answer_list[i][0] = (-1) * answer_list[i][0]; //если мы меняли стремление функции с max на min, то возращаем его назад
                Console.WriteLine($"\n{i+1}-ое решение:");
                if (answer_list[i][0] == F_min) Console.WriteLine("Это оптимальное решение, так как здесь функция минимальна");
                for (int j = 0; j < answer_list[i].Count; ++j)
                {
                    if (j == 0) Console.Write($"F={answer_list[i][j]}  ");
                    if (j != 0) Console.Write($"X{j}={answer_list[i][j]}  "); //печатаем все иксы и F
                }
                Console.Write("\nF = ");
                F = 0;
                for (int g = 0; g < vec_c.Count; ++g) //печатаем значение функции при данных переменных
                {
                    Console.Write($"{vec_c[g]} * {answer_list[i][g+1]}"); //g+1, так как в списке на 1-ом месте значение функции
                    if (g != vec_c.Count - 1) Console.Write(" + ");
                    F += answer_list[i][g+1] * vec_c[g];
                }
                Console.Write($" = {F}\n");
                sum = 0;
                for (int l = 0; l < answer_list[i].Count-1; ++l) //показываем правильность полученных переменных путем подстановки их значений в ограничение
                {
                    Console.Write($"{vec_a[l]} * {answer_list[i][l+1]}");
                    sum += vec_a[l] * answer_list[i][l + 1];
                    if (l != vec_a.Count - 1) Console.Write(" + ");
                }
                Console.Write($" <= {b} \t{sum} <= {b}");
                if (sum <= b) Console.Write(" - верно\n");
            }
        } 

        static void GenerateCombinations(int n, List<double> current, ref List<List<double>> result) //рекурсивный метод, создающий все возможные комбинации 0 и 1 для переменных
        {
            if (current.Count == n) //если комбинация готова, то записываем её в список с комбинациями
            {
                result.Add(new List<double>(current));
                return;
            }
            current.Add(0); 
            GenerateCombinations(n, current, ref result);
            current.RemoveAt(current.Count - 1);
            current.Add(1);
            GenerateCombinations(n, current, ref result);
            current.RemoveAt(current.Count - 1);
        }
    }
}﻿
