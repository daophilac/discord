package com.peanut.discord;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Message;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.view.ContextThemeWrapper;

import com.peanut.androidlib.common.worker.SingleWorker;
import com.peanut.discord.filedealer.InternalFileReader;
import com.peanut.discord.resources.Data;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.tools.JsonBuilder;

import java.io.IOException;

import static com.peanut.discord.MainActivity.themeId;

public class LoginActivity extends AppCompatActivity {
    private APICaller apiCaller;
    private JsonBuilder jsonBuilder;
    private EditText editTextEmail;
    private EditText editTextPassword;
    private String email;
    private String password;
    private TextView textViewSignUp;
    private Button buttonLogin;
    private InternalFileReader internalFileReader;
    public SingleWorker singleWorker;

    @Override
    protected void onDestroy() {
        super.onDestroy();
        singleWorker.quit();
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        MainActivity.sharedPreferences = getSharedPreferences(MainActivity.preferenceName, Context.MODE_PRIVATE);
        themeId = MainActivity.sharedPreferences.getInt("themeId", R.style.Light);
        MainActivity.themeInflater = getLayoutInflater().cloneInContext(new ContextThemeWrapper(this, themeId));
        setTheme(themeId);
        Data.establish(this);
        initializeGlobalVariable();
        if(checkLoggedIn()){
            email = internalFileReader.readConfiguration("email");
            password = internalFileReader.readConfiguration("password");
            internalFileReader.close();
            String json = jsonBuilder.buildLoginJson(email, password);
            apiCaller.setProperties(APICaller.RequestMethod.POST, Route.User.urlLogin, json);
            apiCaller.setOnSuccessListener((connection, response) -> {
                Intent intent = new Intent(this, MainActivity.class);
                intent.putExtra("jsonUser", response);
                startActivity(intent);
                finish();
            }).setOnHttpFailListener((connection, response) -> {
                setContentView(R.layout.activity_login);
                editTextEmail = findViewById(R.id.edit_text_email);
                editTextPassword = findViewById(R.id.edit_text_password);
                textViewSignUp = findViewById(R.id.text_view_sign_up);
                buttonLogin = findViewById(R.id.button_login);
                buttonLogin.setOnClickListener(v -> login());
                textViewSignUp.setOnClickListener(v -> {
                    getSupportFragmentManager()
                            .beginTransaction()
                            .add(R.id.fragment_container, new SignUpFragment())
                            .addToBackStack(null)
                            .commit();
                });
                editTextEmail.setText(email);
                editTextPassword.setText(password);
            }).setOnExceptionListener(e -> {
                Toast.makeText(this, e.getMessage(), Toast.LENGTH_LONG).show();
            }).sendRequest();
        }
        else{
            setContentView(R.layout.activity_login);
            editTextEmail = findViewById(R.id.edit_text_email);
            editTextPassword = findViewById(R.id.edit_text_password);
            textViewSignUp = findViewById(R.id.text_view_sign_up);
            buttonLogin = findViewById(R.id.button_login);
            buttonLogin.setOnClickListener(v -> login());
            textViewSignUp.setOnClickListener(v -> {
                getSupportFragmentManager()
                        .beginTransaction()
                        .add(R.id.fragment_container, new SignUpFragment())
                        .addToBackStack(null)
                        .commit();
            });
        }
    }
    private boolean checkLoggedIn(){
        return internalFileReader.exists();
    }
    private void login (){
        email = editTextEmail.getText().toString();
        password = editTextPassword.getText().toString();
        String json = jsonBuilder.buildLoginJson(email, password);
        apiCaller.setProperties(APICaller.RequestMethod.POST, Route.User.urlLogin, json);
        apiCaller.setOnSuccessListener((connection, response) -> {
            Intent intent = new Intent(LoginActivity.this, MainActivity.class);
            intent.putExtra("jsonUser", response);
            startActivity(intent);
            finish();
            Data.writeUserData(this, email, password);
        }).setOnHttpFailListener((connection, response) -> {
            Toast.makeText(this, response, Toast.LENGTH_LONG).show();
        }).setOnExceptionListener(e -> {
            Toast.makeText(this, e.getMessage(), Toast.LENGTH_LONG).show();
        }).sendRequest();
    }
    private void initializeGlobalVariable(){
        singleWorker = new SingleWorker(MainActivity.LOG_TAG);
        apiCaller = new APICaller();
        jsonBuilder = new JsonBuilder();
        internalFileReader = new InternalFileReader(this, Data.USER_INFORMATION_FILE_PATH);
        internalFileReader.setConfigurationSeparator(":");
    }
}