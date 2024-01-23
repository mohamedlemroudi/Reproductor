package com.example.appmusicandroid.Music

import android.annotation.SuppressLint
import android.content.Context
import android.database.Cursor
import android.net.Uri
import android.os.Environment
import android.provider.MediaStore
import com.example.appmusicandroid.Model.MusicModel
import java.io.File

class FindMusic : IFindMusic {

    override fun getMusicFile(context: Context): ArrayList<MusicModel> {
        val audioList: ArrayList<MusicModel> = ArrayList()

        val musicDirectory = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_MUSIC)
        val musicFiles = musicDirectory.listFiles()

        musicFiles?.forEach { file ->
            val data: String = file.absolutePath
            val title: String = file.nameWithoutExtension
            val album: String = ""
            val artist: String = ""
            val songId: String = file.name.hashCode().toString()
            val albumId: String = ""
            val artistId: String = ""
            val uri = Uri.fromFile(file)
            val uriImage = ""

            audioList.add(MusicModel(data, title, album, artist, songId, albumId, artistId, uri, uriImage))
        }

        return audioList
    }
}
