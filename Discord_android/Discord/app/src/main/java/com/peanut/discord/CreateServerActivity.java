package com.peanut.discord;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import android.widget.Button;
import android.widget.EditText;

import com.peanut.androidlib.common.worker.SingleWorker;
import com.peanut.discord.customview.BackButton;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;

public class CreateServerActivity extends AppCompatActivity {
    private int currentUserId;
    private APICaller apiCaller;
    private BackButton backButton;
    private Button buttonCreate;
    private EditText editTextServerName;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setTheme(MainActivity.themeId);
        setContentView(R.layout.activity_create_server);
        currentUserId = getIntent().getIntExtra("currentUserId", -1);
        apiCaller = new APICaller();

        backButton = findViewById(R.id.back_button);
        buttonCreate = findViewById(R.id.button_create);
        editTextServerName = findViewById(R.id.edit_text_server_name);


        backButton.setOnClickListener(v -> finish());
        buttonCreate.setOnClickListener(v -> {
            if(!editTextServerName.getText().toString().equals("")){
                String json = MainActivity.jsonBuilder.buildServerJson(currentUserId, editTextServerName.getText().toString());
                apiCaller.setProperties(APICaller.RequestMethod.POST, Route.Server.urlAdd, json);
                apiCaller.setOnSuccessListener((connection, response) -> {
                    Intent intent = new Intent(CreateServerActivity.this, MainActivity.class);
                    intent.putExtra("command", MainActivity.IntentCommand.CREATE_SERVER);
                    intent.putExtra("jsonServer", response);
                    startActivity(intent);
                    finish();
                }).sendRequest();
            }
        });
    }
}