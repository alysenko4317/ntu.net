#include <windows.h>
#include <thread>
#include <string>

// Global variables
HWND hwnd;
const UINT WM_SET_TEXT = WM_USER + 1; // Define a custom message ID

#if defined(_UNICODE)
#    define _T(x) L ##x
#else
#    define _T(x) x
#endif

                                      // Function to handle window messages
LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    switch (uMsg) {
    case WM_CLOSE:
        DestroyWindow(hwnd);
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    case WM_SET_TEXT:
        // Handle the custom message to set the window text
        SetWindowText(hwnd, reinterpret_cast<LPCWSTR>(lParam));
        break;
    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
    return 0;
}

// Function to set the text of the window from another thread
void SetWindowTextFromThread(const std::wstring& text)
{
    Sleep(2000);
    // Send a custom message to set the window text
    SendMessage(hwnd, WM_SET_TEXT, 0, reinterpret_cast<LPARAM>(text.c_str()));
}

// Entry point
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    // Register the window class
    WNDCLASSEX wc = {
        sizeof(WNDCLASSEX),
        CS_CLASSDC,
        WindowProc,
        0L,
        0L,
        GetModuleHandle(NULL),
        NULL, NULL, NULL, NULL,
        _T("SimpleWin32App"),
        NULL
    };

    RegisterClassEx(&wc);

    // Create the application's window
    hwnd = CreateWindow(wc.lpszClassName, _T("Simple Win32 App"), WS_OVERLAPPEDWINDOW, 100, 100, 400, 300, NULL, NULL, wc.hInstance, NULL);
    //CreateThread()
    // Initialize the window
    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    // Create a thread to set the text of the window
    std::wstring text = L"Text from another thread!";
    std::thread setTextThread(SetWindowTextFromThread, text);
    //setTextThread.join(); // Wait for the thread to finish

                          // Enter the message loop
    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}
