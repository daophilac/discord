package com.peanut.discord;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.Toast;

import com.peanut.discord.customview.BackButton;
import com.peanut.discord.models.User;
import com.peanut.discord.resources.Data;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;
import com.peanut.discord.tools.JsonBuilder;
import com.peanut.discord.worker.SingleWorker;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SignUpFragment extends Fragment {
    private Context context;
    private APICaller apiCaller;
    private Handler handler;
    private SingleWorker singleWorker;
    private JsonBuilder jsonBuilder;
    private String emailFormat = "\\S+@\\S+\\.\\S+";
    private Pattern pattern = Pattern.compile(emailFormat);
    private BackButton backButton;
    private Button buttonOk;
    private EditText editTextEmail;
    private EditText editTextPassword;
    private EditText editTextConfirmPassword;
    private EditText editTextUserName;
    private EditText editTextFirstName;
    private EditText editTextLastName;
    private RadioButton radioButtonMale;
    private RadioButton radioButtonFemale;
    private User.Gender gender;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        this.context = context;
        this.apiCaller = new APICaller();
        this.jsonBuilder = new JsonBuilder();
        this.singleWorker = new SingleWorker(MainActivity.LOG_TAG);
        this.handler = new Handler(Looper.getMainLooper()){
            @Override
            public void handleMessage(Message msg) {
                Intent intent = new Intent(context, MainActivity.class);
                intent.putExtra("jsonUser", (String)msg.obj);
                getActivity().finish();
                startActivity(intent);
                Data.writeUserData(context, editTextEmail.getText().toString(), editTextPassword.getText().toString());
            }
        };
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = MainActivity.themeInflater.inflate(R.layout.fragment_sign_up, container, false);
        backButton = view.findViewById(R.id.back_button);
        buttonOk = view.findViewById(R.id.button_ok);
        editTextEmail = view.findViewById(R.id.edit_text_email);
        editTextPassword = view.findViewById(R.id.edit_text_password);
        editTextConfirmPassword = view.findViewById(R.id.edit_text_confirm_password);
        editTextUserName = view.findViewById(R.id.edit_text_user_name);
        editTextFirstName = view.findViewById(R.id.edit_text_first_name);
        editTextLastName = view.findViewById(R.id.edit_text_last_name);
        radioButtonMale = view.findViewById(R.id.radio_button_male);
        radioButtonFemale = view.findViewById(R.id.radio_button_female);

        backButton.setOnClickListener(v -> getFragmentManager().popBackStack());
        buttonOk.setOnClickListener(v -> {
            Matcher matcher = pattern.matcher(editTextEmail.getText().toString());
            if(!matcher.find()){
                Toast.makeText(context, context.getText(R.string.invalid_email_format), Toast.LENGTH_LONG).show();
            }
            else{
                if(editTextPassword.getText().toString().equals("")){
                    Toast.makeText(context, context.getText(R.string.password_cannot_be_empty), Toast.LENGTH_LONG).show();
                    return;
                }
                if(!editTextConfirmPassword.getText().toString().equals(editTextPassword.getText().toString())){
                    Toast.makeText(context, context.getText(R.string.password_confirmation_does_not_match), Toast.LENGTH_LONG).show();
                    return;
                }
                if(editTextUserName.getText().toString().equals("")){
                    Toast.makeText(context, context.getText(R.string.user_name_cannot_be_empty), Toast.LENGTH_LONG).show();
                    return;
                }
                if(editTextFirstName.getText().toString().equals("") && editTextLastName.getText().toString().equals("")){
                    Toast.makeText(context, context.getText(R.string.first_and_last_name_are_empty), Toast.LENGTH_LONG).show();
                    return;
                }
                if(radioButtonMale.isChecked()){
                    gender = User.Gender.Male;
                }
                else{
                    gender = User.Gender.Female;
                }
                String email = editTextEmail.getText().toString();
                String password = editTextPassword.getText().toString();
                String userName = editTextUserName.getText().toString();
                String firstName = editTextFirstName.getText().toString();
                String lastName = editTextLastName.getText().toString();
                String json = jsonBuilder.buildUserJson(email, password, userName, firstName, lastName, gender);
                apiCaller.setProperties(handler, APICaller.RequestMethod.POST, Route.urlSignUp, json);
                singleWorker.execute(apiCaller);
            }
        });
        return view;
    }
}
