#include <iostream>
#include <vector>

// Функція, яка додає елемент до вектора за значенням (копія)
void appendByVal(std::vector<int> a, int v) {
    // додаємо елемент до копії вектора
    a.push_back(v);
}

// Функція, яка додає елемент до вектора за посиланням
void appendByRef(std::vector<int>& a, int v) {
    // додаємо елемент до оригінального вектора
    a.push_back(v);
}

// Функція, яка призначає один вектор іншому за посиланням
void assign(std::vector<int>& a, const std::vector<int>& b) {
    a = b;
}

int main() {
    std::vector<int> v1;
    std::vector<int> v2 = { 1, 2, 3 };

    // Додаємо елемент до вектора за значенням (v1 не зміниться)
    appendByVal(v1, 1);

    // Додаємо елемент до вектора за посиланням (v1 зміниться)
    appendByRef(v1, 2);

    // Призначаємо вміст v2 в v1
    //assign(v1, v2);

    // Виводимо вміст v1
    std::cout << "append: ";
    for (const auto& e : v1) {
        std::cout << e << ' ';
    }
    std::cout << std::endl;

    return 0;
}
