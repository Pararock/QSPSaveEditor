#pragma once

#include "Engine.g.h"
#include <chrono>


namespace winrt::QSPLib_CppWinrt::implementation
{
    struct Engine : EngineT<Engine>
    {
        Engine();
        ~Engine();

        /// <summary>
        /// ??? Would be nice a way to send doc to the c# side.
        /// </summary>
        /// <remarks>
        /// This tutorial shows how to document a service using several features.
        /// </remarks>
        static hstring Version();
        static Windows::Foundation::DateTime CompiledDate();

        void CloseGameWorld();

        Windows::Foundation::IAsyncOperation<QSPLib_CppWinrt::Result> LoadGameWorld(Windows::Storage::StorageFile const questGameWorld);
        Windows::Storage::StorageFile CurrentGame() const;

        Windows::Foundation::IAsyncOperationWithProgress<QSPLib_CppWinrt::Result, double> SaveState(Windows::Storage::StorageFile saveGame);
        Windows::Storage::StorageFile CurrentSave() const;

        Windows::Foundation::IAsyncOperation<QSPLib_CppWinrt::Result> OpenSavedGame(Windows::Storage::StorageFile const saveGame);

        QSPLib_CppWinrt::Result ExecuteCommandString(hstring command, bool isRefresh);
        QSPLib_CppWinrt::Result SelectObject(int objectIndex, bool isRefresh);
        QSPLib_CppWinrt::Result SelectAction(int objectIndex, bool isRefresh);
        QSPLib_CppWinrt::Result ExecuteSelectedAction(bool isRefresh);

        QSPLib_CppWinrt::Result  StartGame(bool isRefresh);

        hstring CurrentLocation() const;
        hstring MainView() const;
        bool isFileAccessSafe() const;
        bool isWorldDirty() const;
        bool isGameDirty() const;
        Windows::Foundation::IAsyncOperation<winrt::Windows::Storage::Streams::IInputStream> MainViewStream() const;
        int GameCrc() const;
        Windows::Foundation::Collections::IObservableVector<winrt::QSPLib_CppWinrt::Location> Locations();

        Windows::Foundation::Collections::IObservableVector<winrt::QSPLib_CppWinrt::Variable> Variables();

        winrt::event_token PropertyChanged(Windows::UI::Xaml::Data::PropertyChangedEventHandler const& handler);
        void PropertyChanged(winrt::event_token const& token) noexcept;

        void SendVariablesPropertyChanges(int Index, int virtualIndex, int position);
        static Engine& getInstance();
    private:
        Windows::Storage::StorageFile m_currentFile{ nullptr };
        Windows::Storage::StorageFile m_currentSave{ nullptr };
        Windows::Foundation::Collections::IObservableVector<winrt::QSPLib_CppWinrt::Location> m_locations;
        Windows::Foundation::Collections::IObservableVector<winrt::QSPLib_CppWinrt::Variable> m_variables;
        winrt::event<Windows::UI::Xaml::Data::PropertyChangedEventHandler> m_propertyChanged;
        bool m_haveFullAccessPermissions = false;
        bool m_isGameDirty = false;
        bool m_isWorldDirty = false;

        void InitializeLocation();
        void InitializeVariables();
        static QSPLib_CppWinrt::Result ReturnQSPError(int errorNum);
        static QSPLib_CppWinrt::Result ReturnSuccess();
        static Engine* Instance;
    };
}

namespace winrt::QSPLib_CppWinrt::factory_implementation
{
    struct Engine : EngineT<Engine, implementation::Engine>
    {
    };
}
