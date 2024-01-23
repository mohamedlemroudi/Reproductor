package com.example.appmusicandroid.View

import android.Manifest
import android.content.pm.PackageManager
import android.os.AsyncTask
import android.os.Bundle
import android.widget.Button
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.core.net.toFile
import com.example.appmusicandroid.R
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.MultipartBody
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.RequestBody.Companion.asRequestBody
import okhttp3.Response
import java.io.File
import java.io.IOException

class UploadSongActivity : AppCompatActivity() {

    companion object {
        const val API_BASE_URL = "http://localhost:5095/api/"
        const val API_UPLOAD_AUDIO_URL = "$API_BASE_URL" + "Audio"
    }

    private lateinit var buttonChooseFile: Button
    private lateinit var buttonUpload: Button
    private var selectedFilePath: String? = null

    private val getContent = registerForActivityResult(ActivityResultContracts.GetContent()) { uri ->
        selectedFilePath = uri?.toFile()?.absolutePath
        // Puedes actualizar la interfaz de usuario aquí si es necesario
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_upload_song)

        buttonChooseFile = findViewById(R.id.buttonChooseFile)
        buttonChooseFile.setOnClickListener {
            openFilePicker()
        }

        buttonUpload = findViewById(R.id.buttonUpload)
        buttonUpload.setOnClickListener {
            if (selectedFilePath != null) {
                val audioFile = File(selectedFilePath)
                UploadAudioTask().execute(audioFile)
            } else {
                Toast.makeText(this, "Seleccione un archivo primero", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun openFilePicker() {
        getContent.launch("audio/*")
    }

    private inner class UploadAudioTask : AsyncTask<File, Void, Boolean>() {
        override fun doInBackground(vararg params: File?): Boolean {
            if (params.isNotEmpty()) {
                val file = params[0]
                return uploadAudioFile(file)
            }
            return false
        }

        override fun onPostExecute(result: Boolean) {
            if (result) {
                Toast.makeText(this@UploadSongActivity, "Carga exitosa", Toast.LENGTH_SHORT).show()
            } else {
                Toast.makeText(this@UploadSongActivity, "Error en la carga", Toast.LENGTH_SHORT).show()
            }
        }

        private fun uploadAudioFile(file: File?): Boolean {
            if (file != null) {
                try {
                    val client = OkHttpClient()
                    val request = buildMultipartRequest(file)

                    val response: Response = client.newCall(request).execute()

                    // Verificar la respuesta del servidor
                    if (response.isSuccessful) {
                        return true // Carga exitosa
                    }
                } catch (e: IOException) {
                    e.printStackTrace()
                }
            }
            return false // Carga fallida
        }

        private fun buildMultipartRequest(file: File): Request {
            val requestBody: RequestBody = MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("file", file.name, file.asRequestBody("audio/*".toMediaTypeOrNull()))
                .build()

            return Request.Builder()
                .url(API_UPLOAD_AUDIO_URL)
                .post(requestBody)
                .build()
        }
    }
}
