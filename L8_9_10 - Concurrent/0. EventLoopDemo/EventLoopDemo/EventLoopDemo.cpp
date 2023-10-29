#include <windows.h>

// Global variables
HWND hwnd;

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
    default:
        return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
    return 0;
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

    // Initialize the window
    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    // Enter the message loop
    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}
