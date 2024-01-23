package com.example.appmusicandroid.Util

import kotlin.math.floor

class TimetoMSS {
    companion object{
        fun timestampToMSS(position: Int): String {
            val totalSeconds = floor(position / 1E3).toInt()
            val minutes = totalSeconds / 60
            val remainingSeconds = totalSeconds - (minutes * 60)
            return ("$minutes:$remainingSeconds")
        }
    }
}