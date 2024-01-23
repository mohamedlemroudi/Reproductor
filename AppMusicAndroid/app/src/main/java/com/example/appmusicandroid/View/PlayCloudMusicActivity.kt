package com.example.appmusicandroid.View

import android.content.Intent
import android.os.AsyncTask
import android.os.Bundle
import android.os.Environment
import android.util.Log
import android.widget.ImageView
import androidx.appcompat.app.AppCompatActivity
import com.example.appmusicandroid.Api.ApiService
import com.example.appmusicandroid.R
import com.example.appmusicandroid.databinding.ActivityPlayCloudMusicBinding
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.io.File
import java.io.FileOutputStream
import java.io.IOException
import java.net.HttpURLConnection
import java.net.URL
import android.Manifest
import android.content.ContentValues
import android.content.pm.PackageManager
import android.net.Uri
import android.provider.MediaStore
import android.widget.Toast
import androidx.appcompat.widget.SearchView
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import com.example.appmusicandroid.Music.MediaPlayerController.Companion.context
import java.io.InputStream

class PlayCloudMusicActivity : AppCompatActivity() {
    companion object {
        const val EXTRA_UID = "extra_id"
        const val API_BASE_URL = "http://localhost:5095/api/"
        const val API_AUDIO_URL = "$API_BASE_URL" + "Audio"
        private const val MY_PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE = 123
    }

    private lateinit var binding: ActivityPlayCloudMusicBinding

//    override fun onResume() {
//        super.onResume()
//        search()
//    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityPlayCloudMusicBinding.inflate(layoutInflater)
        setContentView(binding.root)
        val uid = intent.getStringExtra(EXTRA_UID).orEmpty()
        getMusicCloud(uid)

        // RETROCEDER ATRAS
        val iconLeft: ImageView = binding.IconLeft

        // Configurando el clic del ImageView
        iconLeft.setOnClickListener { // Lógica para retroceder aquí (por ejemplo, cerrar la actividad)
            onBackPressed()
        }
    }

    private fun initListener(audioName: String?) {
        binding.btnDownload.setOnClickListener {
            // Verifica si se tienen los permisos
            if (checkSelfPermission(android.Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
                // Se tienen los permisos, realiza la descarga
                DownloadAndSaveAudioTask().execute(audioName)
            } else {
                // Si no se tienen los permisos, solicítalos al usuario
                requestPermissions(arrayOf(android.Manifest.permission.WRITE_EXTERNAL_STORAGE), 1)
            }
        }
    }

//    private fun search(){
//        binding.search.setOnQueryTextListener(object : SearchView.OnQueryTextListener,
//            android.widget.SearchView.OnQueryTextListener {
//            override fun onQueryTextSubmit(query: String?): Boolean {
//                return true
//            }
//            override fun onQueryTextChange(newText: String?): Boolean {
//                return true
//            }
//        })
//    }

    private fun getMusicCloud(uid: String) {
        CoroutineScope(Dispatchers.IO).launch {
            try {
                val myResponse =
                    getRetrofit().create(ApiService::class.java).getSongInformation(uid)
                if (myResponse.isSuccessful) {
                    val response = myResponse.body()
                    val uid = response?.versionOriginalId
                    val songOriginal =
                        uid?.let {
                            getRetrofit().create(ApiService::class.java).getSongInformation(
                                it
                            )
                        }?.body()
                    if(response != null) {
                        runOnUiThread {
                            binding.TitleMusic.text = response.title
                            binding.languageSong.text = "Language: " + response.language
                            binding.durationSong.text = "Duration: " + response.duration
                            if (songOriginal != null) {
                                binding.versionOriginalSong.text = "Song Original: " + songOriginal.title
                            }
                            initListener(response.title)
                        }
                    }

                } else {
                    // Manejar la respuesta no exitosa
                    Log.e("ApiService", "Error: ${myResponse.code()}")
                }
            } catch (e: Exception) {
                // Manejar excepciones
                Log.e("ApiService", "Error: ${e.message}", e)
            }
        }
    }

    private fun getRetrofit(): Retrofit {
        return Retrofit
            .Builder()
            .baseUrl("http://localhost:5095/api/")
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    @Deprecated("Deprecated in Java")
    override fun onBackPressed() {
        super.onBackPressed()
        startActivity(Intent(this, CloudMusicActivity::class.java))
        overridePendingTransition(R.anim.lefttorigth1, R.anim.lefttorigth2)
        finish()
    }

    private inner class DownloadAndSaveAudioTask : AsyncTask<String, Void, Boolean>() {

        override fun doInBackground(vararg params: String?): Boolean {
            if (params.isNotEmpty()) {
                return downloadAndSaveAudio(params[0])
            }
            return false
        }

        override fun onPostExecute(result: Boolean) {
            if (result) {
                Toast.makeText(this@PlayCloudMusicActivity, "Descarga exitosa", Toast.LENGTH_SHORT).show()
            } else {
                Toast.makeText(this@PlayCloudMusicActivity, "Error en la descarga", Toast.LENGTH_SHORT).show()
            }
        }

        private fun downloadAndSaveAudio(audioName: String?): Boolean {
            if (audioName != null) {
                var connection: HttpURLConnection? = null
                var inputStream: InputStream? = null

                try {
                    val apiUrl = "$API_AUDIO_URL/$audioName"
                    val url = URL(apiUrl)
                    connection = url.openConnection() as HttpURLConnection

                    // Verificar y solicitar permisos si es necesario
                    if (!checkPermissions()) {
                        requestPermissions()
                        return false
                    }

                    connection.connect()

                    if (connection.responseCode == HttpURLConnection.HTTP_OK) {
                        inputStream = connection.inputStream

                        // Crear un nuevo archivo en la carpeta de música usando MediaStore
                        val values = ContentValues().apply {
                            put(MediaStore.MediaColumns.DISPLAY_NAME, "$audioName.mp3")
                            put(MediaStore.MediaColumns.MIME_TYPE, "audio/mp3")
                            put(MediaStore.MediaColumns.RELATIVE_PATH, Environment.DIRECTORY_MUSIC)
                        }

                        val contentResolver = contentResolver
                        val audioUri: Uri? = contentResolver.insert(MediaStore.Audio.Media.EXTERNAL_CONTENT_URI, values)

                        audioUri?.let {
                            val outputStream = contentResolver.openOutputStream(audioUri)

                            val buffer = ByteArray(1024)
                            var bytesRead: Int

                            while (inputStream.read(buffer).also { bytesRead = it } != -1) {
                                outputStream?.write(buffer, 0, bytesRead)
                            }

                            outputStream?.close()
                            inputStream.close()

                            return true // Descarga exitosa
                        }
                    }
                } catch (e: IOException) {
                    e.printStackTrace()
                } finally {
                    connection?.disconnect()
                    inputStream?.close()
                }
            }
            return false // Descarga fallida
        }


        private fun checkPermissions(): Boolean {
            // Verificar si se tienen permisos para escribir en el almacenamiento externo
            return ContextCompat.checkSelfPermission(
                this@PlayCloudMusicActivity, // Reemplaza con requireContext() o requireActivity() si estás en otro contexto
                Manifest.permission.WRITE_EXTERNAL_STORAGE
            ) == PackageManager.PERMISSION_GRANTED
        }

        private fun requestPermissions() {
            // Solicitar permisos en tiempo de ejecución
            ActivityCompat.requestPermissions(
                this@PlayCloudMusicActivity, // Reemplaza con requireContext() o requireActivity() si estás en otro contexto
                arrayOf(Manifest.permission.WRITE_EXTERNAL_STORAGE),
                MY_PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE
            )
        }
    }

}
