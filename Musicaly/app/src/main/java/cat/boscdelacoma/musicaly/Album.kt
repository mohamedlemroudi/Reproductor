package cat.boscdelacoma.musicaly

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.ImageView
import android.widget.LinearLayout

class Album : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_album)

        val homeLayout: ImageView = findViewById(R.id.Home)
        val playlistLayout: ImageView = findViewById(R.id.Playlist)
        val subircancionLayout: ImageView = findViewById(R.id.CrearCanco)

        homeLayout.setOnClickListener {
            val intent = Intent(this@Album, MainActivity::class.java)
            startActivity(intent)
        }
        playlistLayout.setOnClickListener {
            val intent = Intent(this@Album, Playlist::class.java)
            startActivity(intent)
        }
        subircancionLayout.setOnClickListener {
            val intent = Intent(this@Album, PujarCanso::class.java)
            startActivity(intent)
        }
    }
}