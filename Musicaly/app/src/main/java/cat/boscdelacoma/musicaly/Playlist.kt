package cat.boscdelacoma.musicaly

import android.content.Intent
import android.os.Bundle
import android.view.Gravity
import android.widget.EditText
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.TextView
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity

class Playlist : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_playlist)

        val homeLayout: ImageView = findViewById(R.id.Home)
        val pujarCanso: ImageView = findViewById(R.id.CrearCanco)
        val album: ImageView = findViewById(R.id.Albums)
        val mainLayout: LinearLayout = findViewById(R.id.LinearLlista)

        findViewById<LinearLayout>(R.id.NewPlaylist).setOnClickListener {
            mostrarDialogo(mainLayout)
        }

            homeLayout.setOnClickListener {
                val intent = Intent(this@Playlist, MainActivity::class.java)
                startActivity(intent)
            }
            pujarCanso.setOnClickListener {
                val intent = Intent(this@Playlist, PujarCanso::class.java)
                startActivity(intent)
            }
            album.setOnClickListener {
                val intent = Intent(this@Playlist, Album::class.java)
                startActivity(intent)
            }
        }

    private fun agregarNuevoLinearLayout(mainLayout: LinearLayout, nombrePlaylist: String) {
        val nuevoLinearLayout = LinearLayout(this)
        nuevoLinearLayout.layoutParams = LinearLayout.LayoutParams(
            1000,
            LinearLayout.LayoutParams.WRAP_CONTENT
        ).apply {
            setMargins(0, 60, 0, 0)
        }
        with(nuevoLinearLayout) {
            orientation = LinearLayout.HORIZONTAL
            gravity = Gravity.CENTER

            addView(ImageView(this@Playlist).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.WRAP_CONTENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                ).apply {
                    setPadding(0, 0, 16, 0)
                }
                setImageResource(R.drawable.listsongs)
            })

            val nestedLinearLayout = LinearLayout(this@Playlist).apply {
                layoutParams = LinearLayout.LayoutParams(
                    0,
                    LinearLayout.LayoutParams.WRAP_CONTENT,
                    1f
                )
                orientation = LinearLayout.VERTICAL

                addView(TextView(this@Playlist).apply {
                    layoutParams = LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.WRAP_CONTENT,
                        LinearLayout.LayoutParams.WRAP_CONTENT
                    )
                    setTextColor(resources.getColor(R.color.black))
                    text = nombrePlaylist
                    textSize = 16f
                })

                addView(TextView(this@Playlist).apply {
                    layoutParams = LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.WRAP_CONTENT,
                        LinearLayout.LayoutParams.WRAP_CONTENT
                    )
                    text = "Num. Cançons - 00:00"
                    textSize = 12f
                })
            }

            addView(nestedLinearLayout)
            addView(ImageView(this@Playlist).apply {
                layoutParams = LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.WRAP_CONTENT,
                    LinearLayout.LayoutParams.WRAP_CONTENT
                )
                setImageResource(R.drawable.more)
            })
        }

        mainLayout.addView(nuevoLinearLayout)
    }

    private fun mostrarDialogo(mainLayout: LinearLayout) {
        AlertDialog.Builder(this).apply {
            setTitle("Nom de la playlist")
            val editTextNombre = EditText(this@Playlist)
            setView(editTextNombre)

            setPositiveButton("Crear") { dialog, which ->
                val nombrePlaylist = editTextNombre.text.toString()
                agregarNuevoLinearLayout(mainLayout, nombrePlaylist)
            }

            setNegativeButton("Cancelar") { dialog, which -> dialog.cancel() }
        }.show()
    }
}
