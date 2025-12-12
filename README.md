# Stack-HanoiTower-Project
# 🏰 Tower of Hanoi – Stack & Recursive Implementation (C#)

This repository contains a complete implementation and documentation of the **Tower of Hanoi** problem using **C#**, including:
- Recursive solution  
- Non-recursive solution using **Stack**  
- Flowcharts  
- Algorithm analysis  
- Documentation for team collaboration  

---

## 📌 1. Project Overview

The **Tower of Hanoi** is a classic algorithmic problem illustrating:
- Recursion  
- Stack operations  
- State transition  
- Exponential time complexity  

This project is built for academic purposes, focusing on learning **Data Structures & Algorithms**, especially **Stack**.

---

## 📂 2. Folder Structure

Stack-HanoiTower-Project
│
├── docs/ # Reports, theory, diagrams, flowcharts
├── src/ # C# source code
│ ├── Recursive/ # Recursive solution
│ ├── StackVersion/ # Non-recursive solution using Stack
│ └── Common/ # Shared code, helpers
│
├── images/ # Flowcharts, architecture diagrams
├── presentation/ # Slides for team presentation
├── test/ # Test cases
│
└── README.md

---

## 🔧 3. Technologies

- **C# (.NET 6/7/8)**
- Visual Studio / VS Code
- Data Structures (Stack)
- Recursion

---

## 🧠 4. Theoretical Background

### ✔ Tower of Hanoi Rules
1. Only one disk can be moved at a time.  
2. No disk may be placed on top of a smaller disk.  
3. Disks may be moved between any of the three rods.

### ✔ Minimum number of moves

\[
S(n) = 2^n - 1
\]

### ✔ Time Complexity

\[
T(n) = O(2^n)
\]

### ✔ Explanation  
To move *n* disks:
- Move *n-1* disks to helper rod  
- Move the largest disk  
- Move *n-1* disks onto the destination rod  

---

## 🧩 5. Algorithms Included

### 🔷 Recursive Approach
```csharp
void Hanoi(int n, char from, char aux, char to)
{
    if (n == 1)
    {
        Console.WriteLine($"Move {from} -> {to}");
        return;
    }

    Hanoi(n - 1, from, to, aux);
    Console.WriteLine($"Move {from} -> {to}");
    Hanoi(n - 1, aux, from, to);
}
class Frame
{
    public int n;
    public char from, aux, to;
    public int stage;  
}
dotnet run --project src/Recursive
dotnet run --project src/StackVersion
