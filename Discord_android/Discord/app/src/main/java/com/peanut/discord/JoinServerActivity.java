package com.peanut.discord;
import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import com.peanut.discord.customview.BackButton;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;

public class JoinServerActivity extends AppCompatActivity {
    private int currentUserId;
    private APICaller apiCaller;
    private BackButton backButton;
    private Button buttonJoin;
    private EditText editTextInstantInvite;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setTheme(MainActivity.themeId);
        setContentView(R.layout.activity_join_server);
        currentUserId = getIntent().getIntExtra("currentUserId", -1);
        apiCaller = new APICaller();
        backButton = findViewById(R.id.back_button);
        buttonJoin = findViewById(R.id.button_join);
        editTextInstantInvite = findViewById(R.id.edit_text_instant_invite);

        backButton.setOnClickListener(v -> finish());
        buttonJoin.setOnClickListener(v -> {
            if(!editTextInstantInvite.getText().toString().equals("")){
                apiCaller.setProperties(APICaller.RequestMethod.GET, Route.InstantInvite.buildGetServerUrl(currentUserId, editTextInstantInvite.getText().toString()));
                apiCaller.setOnSuccessListener((connection, response) -> {
                    Intent intent = new Intent(JoinServerActivity.this, MainActivity.class);
                    intent.putExtra("command", MainActivity.IntentCommand.JOIN_SERVER);
                    intent.putExtra("jsonServer", response);
                    startActivity(intent);
                    finish();
                }).sendRequest();
            }
        });
    }
}