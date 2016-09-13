/* Copyright (C) 2005-2010 Valeriy Argunov (nporep AT mail DOT ru) */
/*
* This library is free software; you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published by
* the Free Software Foundation; either version 2.1 of the License, or
* (at your option) any later version.
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this library; if not, write to the Free Software
* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
*/

#include "../../declarations.h"

#ifdef _DEFAULT_BINDING

#include "../../callbacks.h"
#include "../../actions.h"
#include "../../coding.h"
#include "../../common.h"
#include "../../errors.h"
#include "../../objects.h"
#include "../../text.h"

void qspInitCallBacks()
{
    int i;
    qspIsInCallBack = QSP_FALSE;
    qspIsDisableCodeExec = QSP_FALSE;
    qspIsExitOnError = QSP_FALSE;
    for (i = 0; i < QSP_CALL_DUMMY; ++i)
        qspCallBacks[i] = 0;
}

void qspSetCallBack(int type, QSP_CALLBACK func)
{
    qspCallBacks[type] = func;
}

void qspCallDebug(QSP_CHAR *str)
{
    /* Here, we pass control to the debugger */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_DEBUG])
    {
        qspSaveCallState(&state, QSP_FALSE, QSP_FALSE);
        qspCallBacks[QSP_CALL_DEBUG](str);
        qspRestoreCallState(&state);
    }
}

void qspCallSetTimer(int msecs)
{
    /* Here, we set the timer interval */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SETTIMER])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SETTIMER](msecs);
        qspRestoreCallState(&state);
    }
}

void qspCallRefreshInt(QSP_BOOL isRedraw)
{
    /* Here to perform the interface update */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_REFRESHINT])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_REFRESHINT](isRedraw);
        qspRestoreCallState(&state);
    }
}

void qspCallSetInputStrText(QSP_CHAR *text)
{
    /* Here I set the text of the line */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SETINPUTSTRTEXT])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SETINPUTSTRTEXT](text);
        qspRestoreCallState(&state);
    }
}

void qspCallAddMenuItem(QSP_CHAR *name, QSP_CHAR *imgPath)
{
    /* Here we add a menu item */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_ADDMENUITEM])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_ADDMENUITEM](name, imgPath);
        qspRestoreCallState(&state);
    }
}

void qspCallSystem(QSP_CHAR *cmd)
{
    /* You perform a system call */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SYSTEM])
    {
        qspSaveCallState(&state, QSP_FALSE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SYSTEM](cmd);
        qspRestoreCallState(&state);
    }
}

void qspCallOpenGame(QSP_CHAR *file)
{
    /* It allows the user to select a file */
    /* state of the game to download and load it */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_OPENGAMESTATUS])
    {
        qspSaveCallState(&state, QSP_FALSE, QSP_TRUE);
        qspCallBacks[QSP_CALL_OPENGAMESTATUS](file);
        qspRestoreCallState(&state);
    }
}

void qspCallSaveGame(QSP_CHAR *file)
{
    /* It allows the user to select a file */
    /* to save the state of the game and save */
    /* in its current state */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SAVEGAMESTATUS])
    {
        qspSaveCallState(&state, QSP_FALSE, QSP_TRUE);
        qspCallBacks[QSP_CALL_SAVEGAMESTATUS](file);
        qspRestoreCallState(&state);
    }
}

void qspCallShowMessage(QSP_CHAR *text)
{
    /* It displays a message */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SHOWMSGSTR])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SHOWMSGSTR](text);
        qspRestoreCallState(&state);
    }
}

int qspCallShowMenu()
{
    /* Here, the menu show */
    QSPCallState state;
    int index;
    if (qspCallBacks[QSP_CALL_SHOWMENU])
    {
        qspSaveCallState(&state, QSP_FALSE, QSP_TRUE);
        index = qspCallBacks[QSP_CALL_SHOWMENU]();
        qspRestoreCallState(&state);
        return index;
    }
    return -1;
}

void qspCallShowPicture(QSP_CHAR *file)
{
    /* It shows images */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SHOWIMAGE])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SHOWIMAGE](file);
        qspRestoreCallState(&state);
    }
}

void qspCallShowWindow(int type, QSP_BOOL isShow)
{
    /* Here, show and hide the window */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SHOWWINDOW])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SHOWWINDOW](type, isShow);
        qspRestoreCallState(&state);
    }
}

void qspCallPlayFile(QSP_CHAR *file, int volume)
{
    /* It begins to play a file with a given volume */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_PLAYFILE])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_PLAYFILE](file, volume);
        qspRestoreCallState(&state);
    }
}

QSP_BOOL qspCallIsPlayingFile(QSP_CHAR *file)
{
    /* You check whether a file is played */
    QSPCallState state;
    QSP_BOOL isPlaying;
    if (qspCallBacks[QSP_CALL_ISPLAYINGFILE])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        isPlaying = (QSP_BOOL)qspCallBacks[QSP_CALL_ISPLAYINGFILE](file);
        qspRestoreCallState(&state);
        return isPlaying;
    }
    return QSP_FALSE;
}

void qspCallSleep(int msecs)
{
    /* Here expect specified number of milliseconds */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_SLEEP])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_SLEEP](msecs);
        qspRestoreCallState(&state);
    }
}

int qspCallGetMSCount()
{
    /* You get the number of milliseconds that have passed since the last function call */
    QSPCallState state;
    int count;
    if (qspCallBacks[QSP_CALL_GETMSCOUNT])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        count = qspCallBacks[QSP_CALL_GETMSCOUNT]();
        qspRestoreCallState(&state);
        return count;
    }
    return 0;
}

void qspCallCloseFile(QSP_CHAR *file)
{
    /* Here we carry close file */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_CLOSEFILE])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_CLOSEFILE](file);
        qspRestoreCallState(&state);
    }
}

void qspCallDeleteMenu()
{
    /* It removes the current menu */
    QSPCallState state;
    if (qspCallBacks[QSP_CALL_DELETEMENU])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        qspCallBacks[QSP_CALL_DELETEMENU]();
        qspRestoreCallState(&state);
    }
}

QSP_CHAR *qspCallInputBox(QSP_CHAR *text)
{
    /* Here you enter the text */
    QSPCallState state;
    QSP_CHAR *buffer;
    int maxLen = 511;
    if (qspCallBacks[QSP_CALL_INPUTBOX])
    {
        qspSaveCallState(&state, QSP_TRUE, QSP_FALSE);
        buffer = (QSP_CHAR *)malloc((maxLen + 1) * sizeof(QSP_CHAR));
        *buffer = 0;
        qspCallBacks[QSP_CALL_INPUTBOX](text, buffer, maxLen);
        buffer[maxLen] = 0;
        qspRestoreCallState(&state);
    }
    else
        buffer = qspGetNewText(QSP_FMT(""), 0);
    return buffer;
}

#endif
