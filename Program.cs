using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace MO_LAB4
{
    public class Programm
    {
        public static void Main()
        {
            int b = -16;
            List<double> vec_c = new List<double>() { 6, 9, 5, 8, 5, 8, 6, 8 };
            vec_c.Sort(); //сортируем вектор c от наименьшего к наибольшему значению по правилу I
            List<double> vec_a = new List<double>() { -5, -1, 9, -5, 5, 9, 6, -6 };
            List<double> answer = new List<double>(); //отдельное решение
            List<List<double>> answer_list = new List<List<double>>(); //список со всеми допустимыми решениями
            double F = 0, sum = 0;
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
                    sum += vec_a[j] * combinations[i][j]; //находим сумму иксов для проверки ограничения
                }
                if (sum > b) continue; //проверяем ограничения
                answer = new List<double>(combinations[i]);
                answer.Insert(0, F);
                answer_list.Add(answer); //добавляем ответ в список ответов
            }
            print_answers(answer_list, vec_c, vec_a, b); //вызываем метод печати и проверки на допустимость всех ответов
            bool acceptable_solution = false; //показатель, является ли решение допустимым или нет
            answer = new List<double>() {0, 0, 0, 0, 0, 0, 0, 0, 0 };
            answer_list = new List<List<double>>();
            List<List<double>> no_answer_list = new List<List<double>>(); //список с недопустимыми решениями 1-го подмножества
            List<double> record = new List<double>() { 50000, 0, 0, 0, 0, 0, 0, 0, 0 }; //список с рекордом
            List<double> position = new List<double>();
            Console.WriteLine("\n\nМетод Балаша:");
            while(true) //чтобы в случае преждевременного нахождения решения закончить метод Балаша
            {
                acceptable_solution = Solution_check(ref answer, vec_c, vec_a, b); //проверяем нулевое решение на допустимость
                if (acceptable_solution == true) //если нулевое решение допустимо, то по I правилу метод Балаша завершен
                {
                    record = new List<double>(answer);
                    break; 
                }
                for (int i = 1; i < answer.Count; ++i) //построение 1-го подмножества решений на допустимость и отбрасывание лишних решений
                {
                    answer[i] = 1;
                    acceptable_solution = Solution_check(ref answer, vec_c, vec_a, b); //проверяем решение на допустимость
                    if (answer[0] > record[0]) break; //если значение некоторой функции > рекорда, то дальше мы решения не рассматриваем(Правило II)
                    if (acceptable_solution == true) //если решение допустимо, то записываем его в список допустимых решений и отбрасываем все следующие за ним ветви (Правило I)
                    {
                        answer_list.Add(new List<double>(answer));
                        if (record[0] > answer[0])
                        {
                            record[0] = answer[0];
                            record = new List<double>(answer);
                        }
                        position.Add(i);
                    }
                    if (acceptable_solution == false) no_answer_list.Add(new List<double>(answer));
                    answer[i] = 0;
                }
                if (no_answer_list.Count == 0) break; //если недопустимых решений нет, то последний рекорд - это ответ на метод Балаша
                no_answer_list = new List<List<double>>(one_iteration(ref no_answer_list, ref position, vec_c, vec_a, b, ref record, ref answer_list)); //проходим одну итерацию метода Балаша
                if (no_answer_list.Count == 0) break; 
                no_answer_list = new List<List<double>>(one_iteration(ref no_answer_list, ref position, vec_c, vec_a, b, ref record, ref answer_list)); 
                if (no_answer_list.Count == 0) break; 
                no_answer_list = new List<List<double>>(one_iteration(ref no_answer_list, ref position, vec_c, vec_a, b, ref record, ref answer_list)); 
                if (no_answer_list.Count == 0) break; 
                no_answer_list = new List<List<double>>(one_iteration(ref no_answer_list, ref position, vec_c, vec_a, b, ref record, ref answer_list)); 
                if (no_answer_list.Count == 0) break; 
                no_answer_list = new List<List<double>>(one_iteration(ref no_answer_list, ref position, vec_c, vec_a, b, ref record, ref answer_list)); 
                if (no_answer_list.Count == 0) break; 
                no_answer_list = new List<List<double>>(one_iteration(ref no_answer_list, ref position, vec_c, vec_a, b, ref record, ref answer_list)); 
                if (no_answer_list.Count == 0) break; 
                if (answer_list.Count == 0) //если в дереве нет допустимых решений, то проверяем решение Xi = 1
                {
                    answer = new List<double>() { 0, 1, 1, 1, 1, 1, 1, 1, 1 };
                    acceptable_solution = Solution_check(ref answer, vec_c, vec_a, b); //проверяем решение на допустимость
                    if (acceptable_solution == true) //если решение допустимо, то метод Балаша завершен
                    {
                        answer_list.Add(new List<double>(answer));
                        record = new List<double>(answer);
                    }
                 }
                break;
            }
            int del = 0;
            for (int i = 0; i < answer_list.Count; ++i) //удаляем дубликаты найденных решений, если они есть
            {
                if (answer_list.Count == 1) break;
                for (int j = 0; j < answer_list.Count; ++j)
                {
                    if (answer_list.Count == 1) break;
                    if (j == i) continue;
                    del = 0;
                    for(int z = 0; z < answer_list[i].Count; ++z) 
                    {
                        if (answer_list[i][z] == answer_list[j][z]) ++del;
                    }
                    if (del == 9) answer_list.RemoveAt(i);
                }
            }
            print_answers(answer_list, vec_c, vec_a, b); //вызываем метод печати и проверки на допустимость всех ответов, найденных из метода Балаша
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

        static bool Solution_check(ref List<double> answer, List<double> vec_c, List<double> vec_a, int b)  //метод проверки ограничения
        {
            double sum = 0, F = 0;
            for (int i = 0; i < vec_c.Count; ++i)
            {
                F += vec_c[i] * answer[i+1]; //находим значение функции при данных иксах
                sum += vec_a[i] * answer[i+1]; //находим сумму иксов для проверки ограничения
            }
            answer[0] = F;
            if (sum > b) return false; //проверяем ограничения
            return true;
        }

        static void print_answers(List<List<double>> answer_list, List<double> vec_c, List<double> vec_a, int b) //метод печати допустимых решений и проверки ограничений
        {
            double sum = 0, F = 0, F_min = 50000;
            for (int i = 0; i < answer_list.Count; ++i) //выводим в консоль все допустимые решения и показываем их допустимость
            {
                Console.WriteLine($"\n{i + 1}-ое решение:");
                if (F_min > answer_list[i][0]) F_min = answer_list[i][0];
                for (int j = 0; j < answer_list[i].Count; ++j)
                {
                    if (j == 0) Console.Write($"F={answer_list[i][j]}  ");
                    if (j != 0) Console.Write($"X{j}={answer_list[i][j]}  "); //печатаем все иксы и F
                }
                Console.Write("\nF = ");
                F = 0;
                for (int g = 0; g < vec_c.Count; ++g) //печатаем значение функции при данных переменных
                {
                    Console.Write($"{vec_c[g]} * {answer_list[i][g + 1]}"); //g+1, так как в списке на 1-ом месте значение функции
                    if (g != vec_c.Count - 1) Console.Write(" + ");
                    F += answer_list[i][g + 1] * vec_c[g];
                }
                Console.Write($" = {F}\n");
                sum = 0;
                for (int l = 0; l < answer_list[i].Count - 1; ++l) //показываем правильность полученных переменных путем подстановки их значений в ограничение
                {
                    Console.Write($"{vec_a[l]} * {answer_list[i][l + 1]}");
                    sum += vec_a[l] * answer_list[i][l + 1];
                    if (l != vec_a.Count - 1) Console.Write(" + ");
                }
                Console.Write($" <= {b} \t{sum} <= {b}");
                if (sum <= b) Console.Write(" - верно\n");
            }
            Console.WriteLine($"\nОптимальное решение:");
            for (int i = 0; i < answer_list.Count; ++i) //выводим в консоль оптимальное решение
            {
                if (answer_list[i][0] == F_min) 
                {
                    for(int j = 0; j < answer_list[i].Count; ++j) 
                    {
                        if (j == 0) Console.Write($"\nF = {answer_list[i][j]}   ");
                        if(j != 0) Console.Write($"X{j} = {answer_list[i][j]}   ");
                    }
                }
            }
        }

        static List<List<double>> one_iteration(ref List<List<double>> no_answer_list, ref List<double> position, List<double> vec_c, List<double> vec_a, int b, ref List<double> record, ref List<List<double>> answer_list) //одна итерация метода Балаша
        {
            bool acceptable_solution = false;
            List<List<double>> no_answer_list2 = new List<List<double>>(); //список с недопустимыми решениями некоторого подмножества
            for (int i = 0; i < no_answer_list.Count; ++i) //строим ветви для недопустимых решений
            {
                for (int j = 1; j < no_answer_list[i].Count; ++j)
                {
                    if (position.Contains(j) == true) continue; //по I правилу мы не строим вершины, являющиеся продолжением допустимых решений
                    if (no_answer_list[i][j] == 1) continue;
                    no_answer_list[i][j] = 1;
                    List<double> temp = new List<double>(no_answer_list[i]);
                    acceptable_solution = Solution_check(ref temp, vec_c, vec_a, b); //проверяем решение на допустимость
                    no_answer_list[i] = new List<double>(temp);
                    if (no_answer_list[i][0] > record[0]) break; //если значение некоторой функции > рекорда, то дальше мы решения не рассматриваем(Правило II)
                    if (acceptable_solution == true) //если решение допустимо, то добавляем его в список решений и отбрасываем все следующие за ним ветви (Правило I)
                    {
                        answer_list.Add(new List<double>(no_answer_list[i]));
                        if (record[0] > no_answer_list[i][0]) record = new List<double>(no_answer_list[i]);
                        position.Add(j);
                    }
                    if (acceptable_solution == false) no_answer_list2.Add(new List<double>(no_answer_list[i]));
                    no_answer_list[i][j] = 0;
                }
            }
            return new List<List<double>>(no_answer_list2);
        }
    }
}﻿
