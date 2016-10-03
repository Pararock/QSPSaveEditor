#include "pch.h"
#include "Variable.h"
#include "Variable.g.cpp"
#include "Engine.h"

namespace winrt::QSPLib_CppWinrt::implementation
{
    int32_t static findNameIndexes(int realIndex, int virtualIndex, int position)
    {
        for (auto i = 0; i < qspVars[realIndex].IndsCount; i++)
        {
            if (qspVars[realIndex].Indices[i].Index == position)
            {
                return i;
            }
        }

        return -1;
    }

    void static notifyPropertyChanged(int position)
    {
        Engine::getInstance().SendVariablesPropertyChanges(0, position, 0);
    }

    Variable::Variable() {
        m_position = -1;
        m_virtualIndex = -1;
        m_realIndex = -1;
        m_nameIndex = -1;
    }

    Variable::Variable(int realIndex, int virtualIndex, int position) {
        m_position = position;
        m_virtualIndex = virtualIndex;
        m_realIndex = realIndex;
        m_nameIndex = findNameIndexes(realIndex, virtualIndex, position);
    }

    int32_t Variable::Position() const
    {
        return m_position;
    }

    int32_t Variable::Number() const
    {
        return qspVars[m_realIndex].Values[m_position].Num;
    }

    void Variable::Number(int newNumber)
    {
        if (newNumber != qspVars[m_realIndex].Values[m_position].Num)
        {
            qspVars[m_realIndex].Values[m_position].Num = newNumber;
            notifyPropertyChanged(m_position);
        }
    }

    hstring Variable::Text() const
    {
        if (m_position == -1) return L"";

        if (qspVars[m_realIndex].Values[m_position].Str != nullptr)
        {
            return qspVars[m_realIndex].Values[m_position].Str;
        }

        return L"";

    }

    hstring Variable::Name() const
    {
        if (m_nameIndex == -1) return L"";

        return qspVars[m_realIndex].Indices[m_nameIndex].Str;       
    }

    hstring Variable::BaseName() const
    {
        
        if (m_realIndex == -1) return L"Unitialized";

        return qspVars[m_realIndex].Name;
    }


}