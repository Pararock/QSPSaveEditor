#pragma once
#include "Variable.g.h"


namespace winrt::QSPLib_CppWinrt::implementation
{
    struct Variable : VariableT<Variable>
    {
        Variable();
        Variable(int realIndex, int virtualIndex, int position);

        hstring BaseName() const;
        hstring Name() const;
        int Position() const;

        hstring Text() const;
        int Number() const;
        void Number(int newNumber);

    private:
        int m_position;
        int m_realIndex;
        int m_virtualIndex;
        int m_nameIndex;
    };
}


/*

    runtimeclass Variable
    {
        String BaseName{ get; };
        Int32 Position{ get; };
        String Name{ get; };
        String Text{ get; };

        Int32 Number{ get; };
    };

*/