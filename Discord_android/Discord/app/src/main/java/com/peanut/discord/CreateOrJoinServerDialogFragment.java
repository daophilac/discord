package com.peanut.discord;

import android.app.Dialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.DialogFragment;
import android.support.v4.widget.DrawerLayout;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;

import com.peanut.discord.equipment.Inventory;

import static com.peanut.discord.MainActivity.themeInflater;

public class CreateOrJoinServerDialogFragment extends DialogFragment {
    private Button buttonCreateServer;
    private Button buttonJoinServer;

    @NonNull
    @Override
    public Dialog onCreateDialog(@Nullable Bundle savedInstanceState) {
        Dialog dialog = super.onCreateDialog(savedInstanceState);
        dialog.getWindow().requestFeature(Window.FEATURE_NO_TITLE);
        return dialog;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = themeInflater.inflate(R.layout.dialog_create_or_join_server, container, false);
        buttonCreateServer = view.findViewById(R.id.button_create_server);
        buttonJoinServer = view.findViewById(R.id.button_join_server);

        buttonCreateServer.setOnClickListener(v -> {
            getDialog().dismiss();
            Intent intent = new Intent(getContext(), CreateServerActivity.class);
            intent.putExtra("currentUserId", Inventory.currentUser.getUserName());
            startActivity(intent);
        });
        buttonJoinServer.setOnClickListener(v ->{
            getDialog().dismiss();
            Intent intent = new Intent(getContext(), JoinServerActivity.class);
            intent.putExtra("currentUserId", Inventory.currentUser.getUserName());
            startActivity(intent);
//            drawerLayout.closeDrawer(Gravity.START);
//            getFragmentManager().beginTransaction().add(R.id.drawer_layout, new JoinServerFragment()).addToBackStack(null).commit();
        });
        return view;
    }
}